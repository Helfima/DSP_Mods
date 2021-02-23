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
        private int formId = 6660000;

        public UIController()
        {
            TopMenu topMenu = new TopMenu(this);
            HMEvent.Handler += topMenu.OnEvent;
            AddForm(topMenu);

            MainPanel mainPanel = new MainPanel(this);
            HMEvent.Handler += mainPanel.OnEvent;
            AddForm(mainPanel);

            SelectorItem itemSelector = new SelectorItem(this);
            AddForm(itemSelector);

            SelectorRecipe recipeSelector = new SelectorRecipe(this);
            AddForm(recipeSelector);

            EditionProduct editionProduct = new EditionProduct(this);
            HMEvent.Handler += editionProduct.OnEvent;
            AddForm(editionProduct);

            EditionRecipe editionRecipe = new EditionRecipe(this);
            HMEvent.Handler += editionRecipe.OnEvent;
            AddForm(editionRecipe);

            ChooseRecipe chooseRecipe = new ChooseRecipe(this);
            HMEvent.Handler += chooseRecipe.OnEvent;
            AddForm(chooseRecipe);

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
            if (DSPGame.Game == null || !DSPGame.Game.running) return;
            foreach (HMForm form in forms)
            {
                if (form.Show) form.OnGUI();
            }
        }

        public void Update()
        {
            if (DSPGame.Game == null || !DSPGame.Game.running) return;
            
            
            HMEventQueue.DeQueue();
            
            if (Input.GetKeyDown(KeyCode.I))
            {
                MainPanel main = (MainPanel)forms.Where(form => form is MainPanel).First();
                main.SwitchShow();
            }


            foreach (HMForm form in forms)
            {
                if (form.Show) form.OnUpdate();
            }

            
        }

    }
}
