using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PickyMoo.ESC;
using PickyMoo.Source.System;
using static System.Net.Mime.MediaTypeNames;

namespace PickyMoo
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private ECSManager _ecsManager;
        private Camera2D _camera;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // ECS and camera setup
            _ecsManager = new ECSManager();
            _camera = new Camera2D(GraphicsDevice.Viewport);

            // --- Crop entity ---
            var crop = new Entity();
            var cropTx = new TransformComponent
            {
                EntityId = crop.Id,
                Position = new Vector2(100, 100)
            };
            crop.AddComponent(cropTx);
            _ecsManager.AddComponent(cropTx);

            var cropSpr = new SpriteComponent
            {
                EntityId = crop.Id,
                Texture = Content.Load<Texture2D>("Tut"),
                SourceRectangle = null,
                Origin = Vector2.Zero
            };
          

            var cropScale = new ScaleComponent
            {
                EntityId = crop.Id,
                Scale = new Vector2(0.1f)
            };
            crop.AddComponent(cropScale);
            _ecsManager.AddComponent(cropScale);

            // --- Player entity ---
            var player = new Entity();

            var playerTx = new TransformComponent
            {
                EntityId = player.Id,
                Position = new Vector2(
                    GraphicsDevice.Viewport.Width * 0.5f,
                    GraphicsDevice.Viewport.Height * 0.5f
                )
            };
            player.AddComponent(playerTx);
            _ecsManager.AddComponent(playerTx);

            var playerSpr = new SpriteComponent
            {
                EntityId = player.Id,
                Texture = Content.Load<Texture2D>("player"),
                SourceRectangle = null,
                Origin = new Vector2(16, 16)
            };
            player.AddComponent(playerSpr);
            _ecsManager.AddComponent(playerSpr);

            var playerScale = new ScaleComponent
            {
                EntityId = player.Id,
                Scale = new Vector2(0.2f)
            };
            player.AddComponent(playerScale);
            _ecsManager.AddComponent(playerScale);
            var menuComp = new MenuComponent { EntityId = player.Id };
            player.AddComponent(menuComp);
            _ecsManager.AddComponent(menuComp);

            // --- Dynamic fullscreen map ---
            int tileW = 32, tileH = 32;
            int rows = GraphicsDevice.Viewport.Height / tileH + 1;
            int cols = GraphicsDevice.Viewport.Width / tileW + 1;
            var mapData = new int[rows, cols];
            for (int y = 0; y < rows; y++)
                for (int x = 0; x < cols; x++)
                    mapData[y, x] = (y == 0 || y == rows - 1 || x == 0 || x == cols - 1) ? 0 : 1;

            var mapEntity = new Entity();
            var tilemapComp = new TilemapComponent
            {
                EntityId = mapEntity.Id,
                Map = mapData,
                Tileset = Content.Load<Texture2D>("Tilesheet"), // ensure asset is named Tilesheet
                TileWidth = tileW,
                TileHeight = tileH
            };
            mapEntity.AddComponent(tilemapComp);
            _ecsManager.AddComponent(tilemapComp);
            var inv = new InventoryComponent { EntityId = player.Id };
            player.AddComponent(inv);
            _ecsManager.AddComponent(inv);

            var belt = new ToolbeltComponent { EntityId = player.Id };
            player.AddComponent(belt);
            _ecsManager.AddComponent(belt);
            // --- Input & Camera tag for player ---
            var input = new InputComponent { EntityId = player.Id, Speed = 120f };
            player.AddComponent(input);
            _ecsManager.AddComponent(input);

            var camTag = new CameraComponent { EntityId = player.Id };
            player.AddComponent(camTag);
            _ecsManager.AddComponent(camTag);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Register your systems in this order:
            _ecsManager.AddSystem(new MovementSystem());
            _ecsManager.AddSystem(new InteractionSystem(_ecsManager, Content, 32, 32));
            _ecsManager.AddSystem(new WateringSystem(_ecsManager, 8f, 32, 32, 2));
            _ecsManager.AddSystem(new GrowthSystem());
            _ecsManager.AddSystem(new ToolSwitchSystem());
            _ecsManager.AddSystem(new InventorySystem());
            _ecsManager.AddSystem(new CameraFollowSystem(_camera));
            _ecsManager.AddSystem(new TilemapSystem(_spriteBatch, _camera));
            _ecsManager.AddSystem(new RenderingSystem(_spriteBatch, _camera));

            // HUD *must* go last so it draws on top of everything
            var hoeIcon = Content.Load<Texture2D>("hoe");
            var waterIcon = Content.Load<Texture2D>("wateringCan");
            var axeIcon = Content.Load<Texture2D>("axe");
            var defaultFont = Content.Load<SpriteFont>("DefaultFont");
            _ecsManager.AddSystem(new HUDSystem(
                _spriteBatch,
                defaultFont,
                hoeIcon,
                waterIcon,
                axeIcon
            ));
            var pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
    
            _ecsManager.AddSystem(new WetnessMeterSystem(
                  _spriteBatch, _camera, pixel, 32, 32, 8f
              ));
            _ecsManager.AddSystem(new CraftingSystem());
            _ecsManager.AddSystem(new SellingSystem());
            // **new**: toggleable M‐menu
            _ecsManager.AddSystem(new MenuSystem(
                _spriteBatch, defaultFont, pixel, padding: 10
            ));

        }

        protected override void Update(GameTime gameTime)
        {
            // exit on Esc
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // update movement & camera logic only
            _ecsManager.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // clear screen before drawing
            GraphicsDevice.Clear(Color.DarkOliveGreen);

            // render tilemap & sprites
            _ecsManager.Update(gameTime);

            base.Draw(gameTime);
        }
    }

    public class ECSManager
    {
        private readonly System.Collections.Generic.List<IComponent> _components = new System.Collections.Generic.List<IComponent>();
        private readonly System.Collections.Generic.List<ISystem> _systems = new System.Collections.Generic.List<ISystem>();

        public void AddComponent(IComponent component) => _components.Add(component);
        public void AddSystem(ISystem system) => _systems.Add(system);

        public void Update(GameTime gameTime)
        {
            foreach (var sys in _systems)
                sys.Update(_components, gameTime);
        }
    }
}