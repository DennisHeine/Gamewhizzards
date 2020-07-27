using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Data;

namespace WorldEditor
{
    public class FormControl
    {
        public Form1 mainForm = null;
        public String DataClassType = "";
        public Dictionary<String, GroupBox> additionalBoxes = null;
        public GroupBox mainBox = null;
        public ListBox listBox =null;
        public String WorldClass ="";

        public FormControl(Form1 _mainForm, String _dct, Dictionary<String,GroupBox> _additionalBoxes, GroupBox _mainBox, ListBox _listBox, String _WorldClass)
        {
            mainForm = _mainForm;
            DataClassType = _dct;
            additionalBoxes = _additionalBoxes;
            mainBox = _mainBox;
            listBox = _listBox;
            WorldClass = _WorldClass;
        }

        public void newItem()
        {
            foreach (Control c in mainBox.Controls)
            {
                if (c.GetType() == typeof(TextBox))
                {
                    TextBox c1 = (TextBox)c;
                    c1.Text = "";
                }
            }
            listBox.SelectedIndex = -1;
        }

        public void saveItem()
        {         

            String name = mainForm.txtItemsName_Name.Text;
            //var item = MagicallyCreateInstance("Data.Items.Item");

            var assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)+"\\Data.dll");

            //var assembly = Assembly.GetExecutingAssembly();

           var type = assembly.GetTypes().First(t => t.FullName == "Data.Items.Item");

            var item1= Activator.CreateInstance(type);
            

            //Data.Items.Item item = loadItemFromName(name);
            foreach (Control c in mainBox.Controls)
            {

                if (c.GetType() == typeof(TextBox))
                {
                    if(item1.GetType().GetField(c.Name.Split('_')[1]).FieldType==typeof(String))
                        item1.GetType().GetField(c.Name.Split('_')[1]).SetValue(item1, c.Text);
                    else if (item1.GetType().GetField(c.Name.Split('_')[1]).FieldType == typeof(int))
                        item1.GetType().GetField(c.Name.Split('_')[1]).SetValue(item1, int.Parse(c.Text));
                    else if (item1.GetType().GetField(c.Name.Split('_')[1]).FieldType == typeof(float))
                        item1.GetType().GetField(c.Name.Split('_')[1]).SetValue(item1, float.Parse(c.Text));

                }
            }
            
            ReplaceItem((String)item1.GetType().GetField("Name").GetValue(item1),item1);
        }

  
        
        private static object MagicallyCreateInstance(string className)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var type = assembly.GetTypes()
                .First(t => t.Name == className);

            return Activator.CreateInstance(type);
        }

        public virtual void ReplaceItem(String Name, object obj)
        {
        }

        public virtual object loadItemFromName(string name)
        {
            return null;
        }

    }

    public class FormControlItems:FormControl
    {
        public FormControlItems(Form1 _mainForm, String _dct, Dictionary<String, GroupBox> _additionalBoxes, GroupBox _mainBox, ListBox _listBox, String _WorldClass) :
            base( _mainForm,  _dct,  _additionalBoxes,  _mainBox,  _listBox,  _WorldClass)
        {
        }


        public override void ReplaceItem(String itemName, object item)
        {
            var itms = Data.WorldState.worldState.Items;
            bool found = false;
            foreach (var itm in itms.Values)
            {
                if ((String)itm.GetType().GetField("Name").GetValue(itm) == (String)itemName)
                {
                    found = true;
                    itms[itemName] = (Data.Items.Item)item;
                }
            }
            if (!found)
            {
                itms.Add((String)itemName, (Data.Items.Item)item);
            }
        }

        public override object loadItemFromName(string name)
        {
            foreach (Data.Items.Item itm in WorldState.worldState.Items.Values)
            {
                if (itm.Name == name)
                    return itm;
            }
            return new Data.Items.Item();
        }
    }

}
