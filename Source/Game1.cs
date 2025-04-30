using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PickyMoo.ESC;
using PickyMoo.Source.System;
using PickyMoo.Source.System.Camera;
using PickyMoo.Source.System.Menus;
using PickyMoo.Source.System.Player;
using PickyMoo.Source.System.World;
using static System.Net.Mime.MediaTypeNames;

namespace PickyMoo
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private ECSManager _ecsManager;
        private Camera2D _camera;
        private LocationManager _locationManager;
        private ScreenFadeSystem _fade;

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

            _locationManager = new LocationManager("Farm");

            string[,] farmMap = new string[20, 20];
            string[,] forestMap = new string[20, 20];
            string[,] townMap = new string[20, 20];

            for (int y = 0; y < 20; y++)
                for (int x = 0; x < 20; x++)
                {
                    farmMap[y, x] = "grass";
                    forestMap[y, x] = "dirt";
                    townMap[y, x] = (x + y) % 2 == 0 ? "path" : "grass";
                }

            var tileset = Content.Load<Texture2D>("Tilesheet");

            void RegisterMap(string name, string[,] mapData)
            {
                var terrain = new TerrainComponent
                {
                    EntityId = Guid.NewGuid(),
                    Tileset = tileset,
                    TileSize = 32,
                    TilesPerRow = 4,
                    TileTypes = mapData
                };
                _ecsManager.AddComponent(terrain);
                _locationManager.Register(name, terrain);
            }

            RegisterMap("Farm", farmMap);
            RegisterMap("Forest", forestMap);
            RegisterMap("Town", townMap);
            RegisterMap("Beach", new string[20, 20]); // empty beach

            //// --- Register the current location ---
            //var terrain = new Entity();
            //var terrainComp = new TerrainComponent
            //{
            //    EntityId = terrain.Id,
            //    Tileset = Content.Load<Texture2D>("Tilesheet"),
            //    TileSize = 32,
            //    TilesPerRow = 3,
            //    TileTypes = new string[20, 20]
            //};
            //for (int y = 0; y < 20; y++)
            //    for (int x = 0; x < 20; x++)
            //        terrainComp.TileTypes[y, x] = (y < 5 || x < 5) ? "water" :
            //                                      (x + y) % 5 == 0 ? "path" : "grass";

            //terrain.AddComponent(terrainComp);
            //_ecsManager.AddComponent(terrainComp);
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
            var questLog = new QuestLogComponent { EntityId = player.Id };
            player.AddComponent(questLog);
            _ecsManager.AddComponent(questLog);
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

            var npc = new Entity();
            var npcTx = new TransformComponent
            {
                EntityId = npc.Id,
                Position = new Vector2(100, 100)
            };
            npc.AddComponent(npcTx);
            _ecsManager.AddComponent(npcTx);
            var quest = new QuestComponent
            {
                EntityId = npc.Id,
                QuestId = "quest1",
                Title = "Bring Me Seeds",
                Description = "Deliver 5 seeds to me.",
                ObjectiveType = QuestObjectiveType.DeliverItem,
                RequiredItem = "Seed",
                RequiredAmount = 5,
                Rewards = new Dictionary<string, int>
                {
                    { "Gold", 25 },
                    { "Crop", 2 }
                }
            };
            npc.AddComponent(quest);
            _ecsManager.AddComponent(quest);

            var npcSpr = new SpriteComponent
            {
                EntityId = npc.Id,
                Texture = Content.Load<Texture2D>("NPC"),
                Origin = new Vector2(16, 16)
            };
            npc.AddComponent(npcSpr);
            _ecsManager.AddComponent(npcSpr);

            var npcComp = new NpcComponent
            {
                EntityId = npc.Id,
                Waypoints = new List<Vector2> {
                new Vector2(new Random().NextInt64(1000), new Random().NextInt64(1000)),
                new Vector2(new Random().NextInt64(1000), new Random().NextInt64(1000)),
                new Vector2(new Random().NextInt64(1000), new Random().NextInt64(1000)),
                new Vector2(new Random().NextInt64(1000), new Random().NextInt64(1000))
                },
                WantsToBuy = true,
                HasQuest = true,
                QuestText = "Bring me 5 seeds!"
            };
            npc.AddComponent(npcComp);
            _ecsManager.AddComponent(npcComp);

            var scale = new ScaleComponent
            {
                EntityId = npc.Id,
                Scale = new Vector2(0.15f)
            };
            npc.AddComponent(scale);
            _ecsManager.AddComponent(scale);

           



            // --- Dynamic fullscreen map ---
            int tileW = 32, tileH = 32;
            int rows = GraphicsDevice.Viewport.Height / tileH + 1;
            int cols = GraphicsDevice.Viewport.Width / tileW + 1;
            var mapData = new int[rows, cols];
            for (int y = 0; y < rows; y++)
                for (int x = 0; x < cols; x++)
                    mapData[y, x] = (y == 0 || y == rows - 1 || x == 0 || x == cols - 1) ? 0 : 1;

            var mapEntity = new Entity();
            //var tilemapComp = new TilemapComponent
            //{
            //    EntityId = mapEntity.Id,
            //    Map = mapData,
            //    Tileset = Content.Load<Texture2D>("Tilesheet"), // ensure asset is named Tilesheet
            //    TileWidth = tileW,
            //    TileHeight = tileH
            //};
            //mapEntity.AddComponent(tilemapComp);
            //_ecsManager.AddComponent(tilemapComp);
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
            _ecsManager.AddSystem(new NpcSystem());
            _ecsManager.AddSystem(new NpcInteractionSystem());
            var pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
            _ecsManager.AddSystem(new QuestSystem());

            _fade = new ScreenFadeSystem(_spriteBatch, pixel, _locationManager);

            _ecsManager.AddSystem(new TerrainSystem(_spriteBatch, _camera, _locationManager));
            _ecsManager.AddSystem(new MapTransitionSystem(_locationManager, 20, 20, 32, _fade));
            _ecsManager.AddSystem(_fade);
            _ecsManager.AddSystem(new MinimapSystem(_spriteBatch, pixel, 20, 20, 32));

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