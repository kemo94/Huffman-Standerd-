using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Collections;
namespace huffman
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public Dictionary<string, int> MapDic = new Dictionary<string, int>(); // dictionary for Compression
        public Dictionary<string, string> CompMap = new Dictionary<string, string>(); // Compression result

        public List<string> AllChar = new List<string>(); // All Charcters without duplicate 

        string text = ""; // take data from user
        public int back = 0;
        public int NumOfChar = 0;

        void getDuplicated() // get sum of duplicated char 
        {

            Dictionary<string, int> Map = new Dictionary<string, int>();
            Map.Add((screen.Text[0] + ""), 1);
            var items = from pair in Map orderby pair.Value descending select pair;
            int check = 0;
            for (int i = 1; i < screen.Text.Length; i++)
            {


                items = from pair in Map orderby pair.Value descending select pair;

                foreach (KeyValuePair<string, int> pair in items) // count repeated char
                {
                    if (pair.Key != screen.Text[i] + "") // if not found add it in map

                        check = 1;

                    else // if found increase the counter
                    {
                        Map[pair.Key]++;

                        check = 0;
                        break;
                    }
                }



                if (check == 1)
                {

                    Map.Add(screen.Text[i] + "", 1);
                    check = 0;
                }
            }


            //sort map
            items = from pair in Map orderby pair.Value descending select pair;
            foreach (KeyValuePair<string, int> pair in items)
            {
                MapDic.Add(pair.Key, pair.Value);
                AllChar.Add(pair.Key);
                 
            }
            NumOfChar = Map.Count;
        }

        public Dictionary<string, int> UpdateDic()
        {


            List<string> listChar = new List<string>(MapDic.Keys); 
            Dictionary<string, int> BxMap = new Dictionary<string, int>(); // update map

            int  sum = 0;
            string chars = "";
            for (int indx = 0; indx < listChar.Count; indx ++)
            {
                if (indx == MapDic.Count - 2 || indx == MapDic.Count - 1)
                {
                    chars = listChar[indx] + chars;
                    sum += MapDic[listChar[indx]];
                }

                else

                    BxMap.Add(listChar[indx], MapDic[listChar[indx]]);



                if (indx == MapDic.Count - 1) // add last two string together 

                    BxMap.Add(chars, sum);

            }
            // sort map
            var items = from pair in BxMap orderby pair.Value descending select pair;
            MapDic.Clear();
            AllChar.Clear();

            foreach (KeyValuePair<string, int> pair in items)
            {
                MapDic.Add(pair.Key, pair.Value);
                AllChar.Add(pair.Key);

            }
            return MapDic;
        }

        // Compression
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            text = screen.Text;
            getDuplicated(); // count sum of duplicated char


            Dictionary<string, int> AlterMap = new Dictionary<string, int>(MapDic);
            List<string>  AlterAllChar = new List<string>(AllChar);

            while (MapDic.Count != 2 ) // decrease the map to reach 0 , 1 (left & right) (binary tree) 
            {
                MapDic = UpdateDic();
                if (AllChar.Count == 1)
                    break;
            }
            BinaryTree objBT = new BinaryTree(NumOfChar, AlterMap);
            while ( CompMap.Count != NumOfChar  && AllChar.Count != 1 )
                // send last two strings to add them in binary tree
            CompMap = objBT.updateTree(AllChar[AllChar.Count - 2], AllChar[AllChar.Count - 1] , 0);
            if (AllChar.Count == 1)
                CompMap.Add(AllChar[0], "1");
            screen.Text = "";


            var items = from pair in CompMap orderby pair.Value descending select pair;

            

            for (int i = 0; i < text.Length; i++)
            {

                foreach (KeyValuePair<string, string> pair in items)

                    if (pair.Key == text[i] + "")
                    
                        // print compression result 
                        screen.Text += pair.Value + " ";
                    

            }



        }
        // Decompression
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            string[] compRslt = screen.Text.Split();
            text = screen.Text ;
            screen.Text = "";
            // search in dictionary
            var items = from pair in CompMap orderby pair.Value descending select pair;
            for (int indx = 0; indx < compRslt.Length-1; indx++)
            {


               foreach (KeyValuePair<string, string> pair in items)

                   if (pair.Value == compRslt[indx])
                    
                        screen.Text += pair.Key; 
                    

            }

            // default variable
             MapDic.Clear();
             CompMap.Clear(); 
             AllChar.Clear();  
             BinaryTree x = new BinaryTree(null) ; // head = null

        }
    }
}
