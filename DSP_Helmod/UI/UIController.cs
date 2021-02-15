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
            itemSelector.HMEventHandler += mainPanel.OnEvent;
            AddForm(itemSelector);
            RecipeSelector recipeSelector = new RecipeSelector(this);
            recipeSelector.HMEventHandler += mainPanel.OnEvent;
            AddForm(recipeSelector);
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
            foreach (HMForm form in forms)
            {
                if (form.Show) form.OnUpdate();
            }
        }

    }
}
