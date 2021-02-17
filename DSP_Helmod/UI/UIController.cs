using DSPHelmod.Classes;
using DSPHelmod.UI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DSPHelmod.UI
{
    public class UIController
    {
        private List<HMForm> forms = new List<HMForm>();
        private int formId = 6660000;
        public UIController()
        {
            MainPanel mainPanel = new MainPanel(this);
            AddForm(mainPanel);
            ItemSelector itemSelector = new ItemSelector(this);
            AddForm(itemSelector);
            RecipeSelector recipeSelector = new RecipeSelector(this);
            AddForm(recipeSelector);

            HMEvent.Handler += mainPanel.OnEvent;
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
            if (Input.GetKeyDown(KeyCode.I))
            {
                MainPanel main = (MainPanel)forms.Where(form => form is MainPanel).First();
                main.Show = !main.Show;
            }


            foreach (HMForm form in forms)
            {
                if (form.Show) form.OnUpdate();
            }
        }

    }
}
