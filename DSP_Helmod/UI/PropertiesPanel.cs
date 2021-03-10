using DSP_Helmod.Classes;
using DSP_Helmod.Model;
using DSP_Helmod.UI.Core;
using DSP_Helmod.UI.Gui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DSP_Helmod.UI
{
    public class PropertiesPanel : HMForm
    {
        private List<object> elements = new List<object>();
        protected int toolbarInt = -1;
        protected List<HMForm> toolbarForms;
        protected List<PropertyData> attributeTypes = new List<PropertyData>();
        protected Dictionary<object, Dictionary<string, PropertyData>> datas = new Dictionary<object, Dictionary<string, PropertyData>>();
        protected bool displayNull = false;
        public PropertiesPanel(UIController parent) : base(parent)
        {
            this.name = "Properties Panel";
            this.Caption = "Properties Panel";
            this.IsTool = true;
        }
        public override void OnDoWindow()
        {
            DrawMenu();
            DrawContent();
        }

        public override void OnInit()
        {
            this.windowRect0 = new Rect(500, 200, 1400, 650);
        }

        public override void OnUpdate()
        {
            
        }

        private void DrawMenu()
        {
            if (parent.Forms != null)
            {
                toolbarForms = new List<HMForm>();
                List<string> toolbarString = new List<string>();
                foreach (HMForm form in parent.Forms)
                {
                    if (form != this && form.IsTool)
                    {
                        toolbarForms.Add(form);
                        toolbarString.Add(form.Name);
                    }
                }
                GUILayout.BeginHorizontal(HMStyle.BoxStyle, GUILayout.MaxHeight(20));
                {
                    GUILayout.BeginHorizontal();
                    toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarString.ToArray());
                    GUILayout.EndHorizontal();
                }
                string DisplayNull = displayNull ? "Null On" : "Null Off";
                if (GUILayout.Button(DisplayNull))
                {
                    displayNull = !displayNull;
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                if (toolbarInt > -1)
                {
                    toolbarForms[toolbarInt].SwitchShow(SelectorMode.Properties);
                    toolbarInt = -1;
                }
            }
        }
        private void DrawContent()
        {
            if(elements.Count > 0)
            {
                scrollPosition = GUILayout.BeginScrollView(scrollPosition);
                GUILayout.BeginVertical();
                foreach (PropertyData attributeType in attributeTypes.Where(attr => attr.Name == "name"))
                {
                    List<PropertyData> rowData = new List<PropertyData>();
                    bool isNull = true;
                    foreach (KeyValuePair<object, Dictionary<string, PropertyData>> entry in datas)
                    {
                        Dictionary<string, PropertyData> values = entry.Value;
                        if (values.ContainsKey(attributeType.Name))
                        {
                            PropertyData value = values[attributeType.Name];
                            rowData.Add(value);
                            isNull &= value.IsNull;
                        }
                        else
                        {
                            rowData.Add(null);
                            isNull &= true;
                        }

                    }
                    if (!isNull || displayNull && isNull)
                    {
                        DrawRowButton(attributeType, rowData);
                    }
                }
                foreach (PropertyData attributeType in attributeTypes.Where(attr => attr.Name != "name"))
                {
                    List<PropertyData> rowData = new List<PropertyData>();
                    bool isNull = true;
                    foreach (KeyValuePair<object, Dictionary<string, PropertyData>> entry in datas)
                    {
                        Dictionary<string, PropertyData> values = entry.Value;
                        if (values.ContainsKey(attributeType.Name))
                        {
                            PropertyData value = values[attributeType.Name];
                            rowData.Add(value);
                            isNull &= value.IsNull;
                        }
                        else
                        {
                            rowData.Add(null);
                            isNull &= true;
                        }

                    }
                    if (!isNull || displayNull && isNull)
                    {
                        DrawRow(attributeType, rowData);
                    }
                }
                GUILayout.EndVertical();
                GUILayout.EndScrollView();
            }
        }

        private void DrawRow(PropertyData attributeType, List<PropertyData> rowData)
        {
            
            GUILayout.BeginHorizontal();
            foreach(PropertyData data in rowData)
            {
                string value = data != null && data.Value != null ? data.Value.ToString() : "null";
                DrawColumns(attributeType.Name, value);
            }
            
            GUILayout.EndHorizontal();
        }
        
        
        private void DrawColumns(string name, string value)
        {
            GUILayout.BeginHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Width(200));
            GUILayout.Label(name);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Width(200));
            GUILayout.Label(value);
            GUILayout.EndHorizontal();

            GUILayout.EndHorizontal();
        }

        private void DrawRowButton(PropertyData attributeType, List<PropertyData> rowData)
        {

            GUILayout.BeginHorizontal();
            foreach (PropertyData data in rowData)
            {
                DrawButton(data.Element, attributeType.Name, data.Value.ToString());
            }

            GUILayout.EndHorizontal();
        }

        private void DrawButton(object element, string name, string value)
        {
            GUILayout.BeginHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Width(200));
            GUILayout.Label(name);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Width(200));
            if (GUILayout.Button(value))
            {
                HMEventQueue.EnQueue(this, new HMEvent(HMEventType.RemoveProperties, element));
            }
            GUILayout.EndHorizontal();

            GUILayout.EndHorizontal();
        }

        public void OnEvent(object sender, HMEvent e)
        {
            switch (e.Type)
            {
                case HMEventType.AddProperties:
                    elements.Add(e.Item);
                    PrepareData();
                    break;
                case HMEventType.RemoveProperties:
                    elements.Remove(e.Item);
                    PrepareData();
                    break;
            }
        }

        public override void OnClose()
        {
            
        }

        private void PrepareData()
        {
            datas.Clear();
            attributeTypes.Clear();
            foreach (object element in elements)
            {
                Dictionary<string, PropertyData> data = PrepareData(element);
                if (!datas.ContainsKey(element))
                {
                    datas.Add(element, data);
                    foreach (KeyValuePair<string, PropertyData> entry in data)
                    {
                        attributeTypes.Add(new PropertyData() { Name = entry.Value.Name, Type = entry.Value.Type });
                    }
                }
            }
            attributeTypes.Sort();
            attributeTypes = attributeTypes.Distinct().ToList();
        }

        private Dictionary<string, PropertyData> PrepareData(object element)
        {
            Dictionary<string, PropertyData> data = new Dictionary<string, PropertyData>();

            Type myType = element.GetType();
            MemberInfo[] myMemberInfos = myType.GetMembers();
            for (int i = 0; i < myMemberInfos.Length; i++)
            {
                PropertyData propertyData = new PropertyData();
                MemberInfo myMemberInfo = myMemberInfos[i];
                propertyData.Element = element;
                propertyData.Name = myMemberInfo.Name;
                propertyData.Type = myMemberInfo.MemberType.ToString();
                object valueName = null;
                try
                {
                    valueName = element.GetType().InvokeMember(propertyData.Name,
                        BindingFlags.Public | BindingFlags.GetField | BindingFlags.GetProperty
                        , null, element, null);
                }
                catch { }
                switch (valueName)
                {
                    case Array array:
                        propertyData.Value = "";
                        for (int index = 0; index < array.Length; index++)
                        {
                            object arrayValue = array.GetValue(index);
                            if (arrayValue != null)
                                propertyData.Value += arrayValue.ToString() + ",";
                        }
                        break;
                    case PrefabDesc prefabDesc:
                        propertyData.Value = PrepareString(prefabDesc);
                        break;
                    case ItemProto itemProto:
                        propertyData.Value = PrepareString(itemProto);
                        break;
                    case RecipeProto recipeProto:
                        propertyData.Value = PrepareString(recipeProto);
                        break;
                    case TechProto techProto:
                        propertyData.Value = PrepareString(techProto);
                        break;
                    default:
                        propertyData.Value = valueName;
                        break;
                }
                data.Add(propertyData.Name, propertyData);
            }
            return data;
        }

        private string PrepareString(object element)
        {
            StringBuilder builder = new StringBuilder();
            if (element != null)
            {
                SortedDictionary<string, string> values = new SortedDictionary<string, string>();
                Type myType = element.GetType();
                MemberInfo[] myMemberInfos = myType.GetMembers();
                for (int i = 0; i < myMemberInfos.Length; i++)
                {
                    MemberInfo myMemberInfo = myMemberInfos[i];
                    object valueName = null;
                    try
                    {
                        valueName = element.GetType().InvokeMember(myMemberInfo.Name,
                            BindingFlags.Public | BindingFlags.GetField | BindingFlags.GetProperty
                            , null, element, null);
                    }
                    catch { }
                    if (valueName != null)
                    {
                        string result = "";
                        switch (valueName)
                        {
                            case Array array:
                                for (int index = 0; index < array.Length; index++)
                                {
                                    object arrayValue = array.GetValue(index);
                                    if(arrayValue != null)
                                        result += arrayValue.ToString() + ",";
                                }
                                break;
                            default:
                                result = valueName.ToString();
                                break;
                        }
                        values.Add(myMemberInfo.Name, result);
                    }
                }
                foreach (KeyValuePair<string,string> entry in values)
                {
                    if(entry.Value != "" && entry.Value != "0")
                        builder.AppendLine($"{entry.Key}:{entry.Value}");
                }
            }
            return builder.ToString();
        }
    }

    public class PropertyData: IEquatable<PropertyData>, IComparable<PropertyData>
    {
        public object Element;
        public string Name;
        public object Value;
        public string Type;

        public bool IsNull
        {
            get { return Value == null; }
        }
        public bool Equals(PropertyData other)
        {
            return Name.Equals(other.Name) && Type.Equals(other.Type);
        }
        public int CompareTo(PropertyData other)
        {
            // A null value means that this object is greater.
            if (other == null)
                return 1;
            else
                return this.Name.CompareTo(other.Name);
        }
        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Type.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Name}:{Type}:{Value}";
        }
    }
}
