using DSP_Helmod.Classes;
using DSP_Helmod.Converter;
using DSP_Helmod.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSP_Helmod.Model
{
    public class Database
    {
        private static string SavePath;

        public static DataModel DataModel = new DataModel();

        private static List<Factory> factories = new List<Factory>();

        private static List<Factory> factoriesOrbiter = new List<Factory>();

        private static List<Factory> factoriesVein = new List<Factory>();

        private static List<Item> logistics = new List<Item>();

        private static List<IRecipe> recipes = new List<IRecipe>();

        private static Dictionary<int, List<IRecipe>> recipesByProduct = new Dictionary<int, List<IRecipe>>();

        private static Dictionary<string, List<IRecipe>> recipesByGroup = new Dictionary<string, List<IRecipe>>();

        public static Dictionary<string, List<IRecipe>> RecipesByGroup
        {
            get { return recipesByGroup; }
        }
        public static List<Item> LogisticItems
        {
            get { return logistics.Where(item => item.Proto.prefabDesc.isBelt).ToList(); }
        }

        public static List<Factory> Factories
        {
            get { return factories; }
        }

        public static List<Factory> FactoriesOrbiter
        {
            get { return factoriesOrbiter; }
        }
        public static List<Factory> FactoriesVein
        {
            get { return factoriesVein; }
        }
        public static void Load()
        {
            foreach (ItemProto itemProto in LDB.items.dataArray)
            {
                switch (itemProto.Type)
                {
                    case EItemType.Production:
                        factories.Add(new Factory(itemProto, 1));
                        break;
                    case EItemType.Logistics:
                        logistics.Add(new Item(itemProto, 1));
                        break;
                }
                if (itemProto.BuildInGas)
                {
                    factoriesOrbiter.Add(new Factory(itemProto, 1));
                }
                if (itemProto.prefabDesc.oilMiner || itemProto.prefabDesc.veinMiner)
                {
                    factoriesVein.Add(new Factory(itemProto, 1));
                }
                // 1011=Fire Ice
                if (itemProto.miningFrom == "Gas giant orbit" || itemProto.ID == 1011)
                {
                    RecipeOrbit recipe = new RecipeOrbit(itemProto, 1);
                    recipes.Add(recipe);
                    AddRecipeByProduct(recipe);
                    AddRecipeByGroup(recipe, "Orbit");
                }
                if (itemProto.miningFrom == "Ocean")
                {
                    RecipeOcean recipe = new RecipeOcean(itemProto, 1);
                    recipes.Add(recipe);
                    AddRecipeByProduct(recipe);
                    AddRecipeByGroup(recipe, "Ocean");
                }
                if (RecipeCustom.Ids.Contains(itemProto.ID))
                {
                    RecipeCustom recipe = new RecipeCustom(itemProto.ID, 1);
                    recipes.Add(recipe);
                    AddRecipeByProduct(recipe);
                    AddRecipeByGroup(recipe, "Custom");
                }
            }
            foreach (RecipeProto recipeProto in LDB.recipes.dataArray)
            {
                Recipe recipe = new Recipe(recipeProto, 1);
                recipes.Add(recipe);
                AddRecipeByProduct(recipe);
                AddRecipeByGroup(recipe, recipeProto.Type.ToString());
            }
            foreach (VeinProto veinProto in LDB.veins.dataArray)
            {
                RecipeVein recipe = new RecipeVein(veinProto, 1);
                recipes.Add(recipe);
                AddRecipeByProduct(recipe);
                AddRecipeByGroup(recipe, "Vein");
            }

            factories.Sort(delegate (Factory factory1, Factory factory2)
            {
                return factory1.Speed.CompareTo(factory2.Speed);
            });
            factoriesVein.Sort(delegate (Factory factory1, Factory factory2)
            {
                return factory1.Speed.CompareTo(factory2.Speed);
            });
            FactoriesOrbiter.Sort(delegate (Factory factory1, Factory factory2)
            {
                return factory1.Speed.CompareTo(factory2.Speed);
            });
            // load file
            LoadModel();
        }

        internal static void AddRecipeByProduct(IRecipe recipe)
        {
            foreach (IItem item in recipe.Products)
            {
                if (!recipesByProduct.ContainsKey(item.Id))
                {
                    recipesByProduct.Add(item.Id, new List<IRecipe>());
                }
                List<IRecipe> recipes = recipesByProduct[item.Id];
                recipes.Add(recipe);
            }
        }
        internal static void AddRecipeByGroup(IRecipe recipe, string group)
        {
            if (!recipesByGroup.ContainsKey(group))
            {
                recipesByGroup.Add(group, new List<IRecipe>());
            }
            List<IRecipe> recipes = recipesByGroup[group];
            recipes.Add(recipe);
        }

        public static IRecipe SelectRecipe(int id)
        {
            IRecipe recipe = recipes.Where(element => element.Id == id).First();
            return recipe.Clone();
        }
        public static IRecipe SelectRecipe(string type, int id)
        {
            IRecipe recipe = recipes.Where(element => element.GetType().Name == type && element.Id == id).FirstOrDefault();
            return recipe?.Clone();
        }
        public static T SelectRecipe<T>(int id)
        {
            IRecipe recipe = recipes.Where(element => element.Id == id && element is T).First();
            return (T)recipe.Clone();
        }
        public static List<IRecipe> SelectRecipeByProduct(IItem item)
        {
            if (!recipesByProduct.ContainsKey(item.Id)) return null;
            List<IRecipe> recipes = recipesByProduct[item.Id];
            return recipes;
        }

        public static void LoadModel()
        {
            if (SavePath == null)
            {
                // placement dans AppData
                SavePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DSP_Helmod/datamodel.xml");
                try
                {
                    // ancien chemin
                    string oldSave = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DSP_Helmod/datamodel.xml");
                    if (System.IO.File.Exists(oldSave))
                    {
                        string oldFolder = System.IO.Path.GetDirectoryName(oldSave);
                        string newFolder = System.IO.Path.GetDirectoryName(SavePath);
                        System.IO.Directory.Move(oldFolder, newFolder);
                    }
                }
                catch { }
            }
            DataModel dataModel = DataModelConverter.ReadXml(SavePath);
            if (dataModel != null)
            {
                DataModel = dataModel;
                Compute compute = new Compute();
                foreach (Nodes sheet in DataModel.Sheets)
                {
                    compute.Update(sheet);
                }
            }
            HMEventQueue.EnQueue(dataModel, new HMEvent(HMEventType.LoadedModel, dataModel));
        }
        public static void SaveModel()
        {
            if (SavePath != null)
            {
                DataModelConverter.WriteXml(SavePath, DataModel);
            }
        }
    }

    public class GameData
    {
        public static UIGame UIGame = BGMController.instance.uiGame;

        public static bool InGame
        {
            get { return UIGame != null && UIGame.gameData != null; }
        }
        public static double MiningCostRate
        {
            get {
                if (UIGame == null || UIGame.gameData == null) return 1;
                return UIGame.gameData.history.miningCostRate;
            }
        }
        public static double MiningSpeedScale
        {
            get {
                if (UIGame == null || UIGame.gameData == null) return 1;
                return UIGame.gameData.history.miningSpeedScale;
            }
        }

        public static List<PlanetData> Planets
        {
            get
            {
                if (UIGame == null || UIGame.gameData == null) return new List<PlanetData>();
                List<PlanetData> planets = new List<PlanetData>();
                foreach (StarData starData in UIGame.gameData.galaxy.stars)
                {
                    foreach (PlanetData planetData in starData.planets)
                    {
                        planets.Add(planetData);
                    }
                }
                return planets;
            }
        }

        public static List<StarData> Stars
        {
            get
            {
                if (UIGame == null || UIGame.gameData == null) return new List<StarData>();
                List<StarData> stars = new List<StarData>();
                foreach (StarData starData in UIGame.gameData.galaxy.stars)
                {
                    stars.Add(starData);
                }
                return stars;
            }
        }
    }
}
