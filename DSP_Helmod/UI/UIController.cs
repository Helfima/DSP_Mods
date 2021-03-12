using DSP_Helmod.Classes;
using DSP_Helmod.UI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DSP_Helmod.UI
{
    public class UIController : MonoBehaviour
    {
        private List<HMForm> forms = new List<HMForm>();
        MainTooltip mainTooltip;
        private int formId = 6660000;
        public Vector2 TooltipPosition;
        public string Tooltip;
        public bool loaded = false;

        public UIController()
        {
            TopMenu topMenu = new TopMenu(this);
            HMEvent.Handler += topMenu.OnEvent;
            AddForm(topMenu);

            mainTooltip = new MainTooltip(this);

            MainPanel mainPanel = new MainPanel(this);
            HMEvent.Handler += mainPanel.OnEvent;
            AddForm(mainPanel);

            AboutPanel aboutPanel = new AboutPanel(this);
            HMEvent.Handler += aboutPanel.OnEvent;
            AddForm(aboutPanel);
#if DEBUG
            PropertiesPanel propertiesPanel = new PropertiesPanel(this);
            HMEvent.Handler += propertiesPanel.OnEvent;
            AddForm(propertiesPanel);

            SelectorItem itemSelector = new SelectorItem(this);
            AddForm(itemSelector);

            SelectorVein veinSelector = new SelectorVein(this);
            AddForm(veinSelector);

            SelectorVege vegeSelector = new SelectorVege(this);
            AddForm(vegeSelector);

            SelectorPlanet planetSelector = new SelectorPlanet(this);
            AddForm(planetSelector);
#endif

            SelectorRecipe recipeSelector = new SelectorRecipe(this);
            AddForm(recipeSelector);

            EditionPreference editionPreference = new EditionPreference(this);
            HMEvent.Handler += editionPreference.OnEvent;
            AddForm(editionPreference);

            EditionProduct editionProduct = new EditionProduct(this);
            HMEvent.Handler += editionProduct.OnEvent;
            AddForm(editionProduct);

            EditionRecipe editionRecipe = new EditionRecipe(this);
            HMEvent.Handler += editionRecipe.OnEvent;
            AddForm(editionRecipe);

            ChooseRecipe chooseRecipe = new ChooseRecipe(this);
            HMEvent.Handler += chooseRecipe.OnEvent;
            AddForm(chooseRecipe);

            HMEvent.Handler += OnEvent;
        }

        private void Load()
        {
            Model.Database.Load();
            loaded = true;
        }

        public List<HMForm> Forms
        {
            get { return forms; }
        }

        private void AddForm(HMForm form)
        {
            formId++;
            form.id = formId;
            forms.Add(form);
        }

        public void OnGUI()
        {
            if (DSPGame.Game == null || !DSPGame.Game.running || !Model.GameData.InGame) return;
            
            HMEventQueue.DeQueue();

            foreach (HMForm form in forms)
            {
                if (form.Show) form.OnGUI();
            }
            if(mainTooltip.Show) mainTooltip.OnGUI();

        }

        /// <summary>
        /// Run at first
        /// </summary>
        public void Update()
        {
            if (DSPGame.Game == null || !DSPGame.Game.running || !Model.GameData.InGame) return;
            if (!loaded) Load();

            //HMEventQueue.DeQueue();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseAll();
            }

            if (Input.GetKeyDown(Settings.Instance.OpenCloseKeyCode))
            {
                MainPanel main = (MainPanel)forms.Where(form => form is MainPanel).First();
                main.SwitchShow();
            }


            foreach (HMForm form in forms)
            {
                if (form.Show) form.OnUpdate();
            }
            mainTooltip.OnUpdate();
        }

        public void CloseAll()
        {
            foreach (HMForm form in forms)
            {
                if (!form.IsPersistant) form.Close();
            }
        }

        public void OnEvent(object sender, HMEvent e)
        {
            switch (e.Type)
            {
                case HMEventType.OpenClose:
                    Type type = e.GetItem<Type>();
                    foreach (HMForm form in forms)
                    {
                        if (form.GetType().Equals(type)) form.SwitchShow();
                    }
                    break;
            }
        }
    }
}
