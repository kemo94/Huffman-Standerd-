using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace huffman
{
    class BinaryTree : MainWindow
    {
     
        Dictionary<string, int> AlterDec; // Alternative dictionary

        public int notFnsh = 0; // to prevent infinite loop 
        List<Node> rest_Nodes = new List<Node>(); // all nodes that hava more than one char
        Node restNd; // parent of rest node

        public BinaryTree(int NumOfChar, Dictionary<string, int> AlterDec)
        {
            this.NumOfChar = NumOfChar;
            this.AlterDec = AlterDec;

        }
        public BinaryTree(Node head) { this.head = head; }


        public class Node  // binary tree
        {
            public string symbol; // char
            public Node left, right;
            public string code; // 0 or 1 or.... so on
        }

        public Node head = null;
        int NumOfNode = 0; // num of nodes to compare it with num of chars

        void addNode(Node Top, Node Right, Node Left, string LeftSymbol, string RightSymbol)
        {

            Top.symbol = ""; // empty symbol of parent node
            // right
            Right.symbol = RightSymbol;
            Right.code = Top.code + "1";
            Right.left = null;
            Right.right = null;
            // left
            Left.symbol = LeftSymbol;
            Left.code = Top.code + "0";
            Left.left = null;
            Left.right = null;

            Top.right = Right;
            Top.left = Left;


            // add symbol to the result if just one char
            if (Right.symbol.Length == 1)
            {
                CompMap.Add(Right.symbol, Right.code); // Compression result
                NumOfNode++; // increase num of nodes
            }
            if (Left.symbol.Length == 1)
            {

                CompMap.Add(Left.symbol, Left.code);
                NumOfNode++;
            }


            MapDic = new Dictionary<string, int>(AlterDec); // update dictionary 
            AllChar = new List<string>(AlterDec.Keys);
        }

        public Dictionary<string, string> updateTree(string LeftSymbol, string RightSymbol, int check)
        {

            Node Top = new Node();
            Node Right = new Node();
            Node Left = new Node();

            if (head == null)
            {
                Top.code = "";
                addNode(Top, Right, Left, LeftSymbol, RightSymbol);
                head = Top;
            }
            else
            {
                if (check == 0) // when check = 0 that mean the Top start from head 
                {
                    MapDic = new Dictionary<string, int>(AlterDec);
                    AllChar = new List<string>(AlterDec.Keys);
                    Top = head;
                }
                if (check == 1) // when check = 1 that mean the Top start from parent of rest node (restNd) 

                    Top = restNd;

                Right = Top.right;
                Left = Top.left;


                int chck = 0; // to check if the symbol of right node or left node has been distributed
                // it will stop when num of all char (NumOfChar) = num of node (NumOfNode ) 
                while (NumOfChar != NumOfNode && AllChar.Count < NumOfChar + 1)
                {
                    if (NumOfNode + 1 == NumOfChar)
                        AllChar = new List<string>(AlterDec.Keys);


                    // check  if last two char equal  the right symbol (node)
                    if (Right.symbol != null)
                        if (Right.symbol.Length > 1 && AllChar[AllChar.Count - 1] + AllChar[AllChar.Count - 2] == Right.symbol)
                        {
                            if (Top.left.symbol.Length > 1)
                                rest_Nodes.Add(Top);

                            Top = Top.right;
                            Right = new Node();
                            Left = new Node();
                            addNode(Top, Right, Left, AllChar[AllChar.Count - 2], AllChar[AllChar.Count - 1]);

                            chck = 1; // that mean the symbol has been distributed so continue


                        }
                    // check  if last two char equal  the left symbol (node)
                    if (Left.symbol != null)
                        if (Left.symbol.Length > 1 && AllChar[AllChar.Count - 1] + AllChar[AllChar.Count - 2] == Left.symbol)
                        {

                            if (Top.right.symbol.Length > 1)
                                rest_Nodes.Add(Top);

                            Top = Top.left;
                            Right = new Node();
                            Left = new Node();

                            addNode(Top, Right, Left, AllChar[AllChar.Count - 2], AllChar[AllChar.Count - 1]);

                            chck = 1; // that mean the symbol has been distributed so continue

                        }

               
                    if (Right.symbol != null)
                        if (Right.symbol.Length == 1)
                            if (Top.left.symbol.Length > 1 && rest_Nodes.IndexOf(Top) == -1 ) 
                                rest_Nodes.Add(Top);

                    if (Left.symbol != null)
                        if (Left.symbol.Length == 1)
                            if (Top.right.symbol.Length > 1 && rest_Nodes.IndexOf(Top) == -1 )
                                rest_Nodes.Add(Top);



                    notFnsh++;
                    if (notFnsh > 3)
                    {
                        AllChar = new List<string>(AlterDec.Keys);
                        notFnsh = 0;
                    }
                    // to updete dictionary (MapDec)
                    else if (chck == 1 || (Left.symbol.Length >= AllChar[AllChar.Count - 2].Length + AllChar[AllChar.Count - 1].Length ||
                        Right.symbol.Length >= AllChar[AllChar.Count - 2].Length + AllChar[AllChar.Count - 1].Length))
                    {
                        chck  = 0 ;
                        // to updete dictionary (MapDec)
                        MapDic = UpdateDic();
                        AllChar = new List<string>(MapDic.Keys);
                    }
                    else  
                         break;

              
                }
            }


                 while ( rest_Nodes.Count != 0 ) // get rest nodes
                {
                    restNd = rest_Nodes[0];
                    rest_Nodes.Remove(rest_Nodes[0]); // delete the taken rest node

                    if (restNd.left.symbol.Length > 1 || restNd.right.symbol.Length > 1 )
                    updateTree(AllChar[AllChar.Count - 1], AllChar[AllChar.Count - 2], 1);

                }
            return CompMap;
        }

    }
}

