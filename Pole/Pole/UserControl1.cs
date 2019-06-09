using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pole
{
    class GameButton : Button
    {

        public bool IsClear, IsCross, IsZero;

        public GameButton()
        {
            Text = "";
            Font = new Font(Font.Name, 30, Font.Style);
            IsClear = true;
            IsCross = IsZero = false;
        }
        public void SetCross()
        {
            Text = "X";
            IsCross = true; IsClear = IsZero = false;
        }
        public void SetNull()
        {
            Text = "O";
            IsClear = IsCross = false; IsZero = true;
        }

        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.ResumeLayout(false);

        }

        public void SetClear()
        {
            Text = "";
            IsClear = true; IsCross = IsZero = false;
        }

        private Button button1;
    }

    public partial class UserControl1 : UserControl
    {

        GameButton[,] gameButtons = new GameButton[3, 3];
        RadioButton computer = new RadioButton();
        RadioButton human = new RadioButton();
        public delegate void FinishEventHandler();
        public event FinishEventHandler OnFinish;
        bool active = true;
        int Moves = 0;
        bool mode = true;

        public UserControl1()
        {
            InitializeComponent();
            this.ClientSizeChanged += new EventHandler(PoleClientSizeChanged);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    gameButtons[i, j] = new GameButton();
                    gameButtons[i, j].Parent = this;
                    gameButtons[i, j].Click += new EventHandler(GameButtonClick);
                }
            }
        
        }
     
        void PoleClientSizeChanged(object sender, EventArgs e)
        { //Rozmiary przyciskow  
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    gameButtons[i, j].Width = ClientRectangle.Width / 4;
                    gameButtons[i, j].Height = ClientRectangle.Height / 3;
                    gameButtons[i, j].Left = i * gameButtons[i, j].Width;
                    gameButtons[i, j].Top = j * gameButtons[i, j].Height;
                }

            //human.Location = new Point(100,50);
        }
        void CheckWin()
        {//sprawdzamy, kto wygral
            if (gameButtons[0, 0].IsCross && gameButtons[0, 1].IsCross && gameButtons[0, 2].IsCross
                || gameButtons[1, 0].IsCross && gameButtons[1, 1].IsCross && gameButtons[1, 2].IsCross
                || gameButtons[2, 0].IsCross && gameButtons[2, 1].IsCross && gameButtons[2, 2].IsCross
                || gameButtons[0, 0].IsCross && gameButtons[1, 0].IsCross && gameButtons[2, 0].IsCross
                || gameButtons[0, 1].IsCross && gameButtons[1, 1].IsCross && gameButtons[2, 1].IsCross
                || gameButtons[0, 2].IsCross && gameButtons[1, 2].IsCross && gameButtons[2, 2].IsCross
                || gameButtons[0, 0].IsCross && gameButtons[1, 1].IsCross && gameButtons[2, 2].IsCross
                || gameButtons[0, 2].IsCross && gameButtons[1, 1].IsCross && gameButtons[2, 0].IsCross)
            {
                active = false;
                MessageBox.Show("Winner X");
            }
            if (gameButtons[0, 0].IsZero && gameButtons[0, 1].IsZero && gameButtons[0, 2].IsZero
                || gameButtons[1, 0].IsZero && gameButtons[1, 1].IsZero && gameButtons[1, 2].IsZero
                || gameButtons[2, 0].IsZero && gameButtons[2, 1].IsZero && gameButtons[2, 2].IsZero
                || gameButtons[0, 0].IsZero && gameButtons[1, 0].IsZero && gameButtons[2, 0].IsZero
                || gameButtons[0, 1].IsZero && gameButtons[1, 1].IsZero && gameButtons[2, 1].IsZero
                || gameButtons[0, 2].IsZero && gameButtons[1, 2].IsZero && gameButtons[2, 2].IsZero
                || gameButtons[0, 0].IsZero && gameButtons[1, 1].IsZero && gameButtons[2, 2].IsZero
                || gameButtons[0, 2].IsZero && gameButtons[1, 1].IsZero && gameButtons[2, 0].IsZero)
            {
                active = false;
                MessageBox.Show("Winner O");
            }
            
        }
        public void Clear() //metoda do czyszczenia starej gry
        {
            for (int r = 0; r < 3; r++)
                for (int c = 0; c < 3; c++)
                    gameButtons[r, c].SetClear();
            active = true; Moves = 0;
        }
        
        void GameButtonClick(object sender, EventArgs e)
        {
            Button senderB = (Button)sender;
            if ((senderB as GameButton).IsClear)
                if (active)
                {
                    (senderB as GameButton).SetCross();

                    //    else
                    //    {
                    //        (senderB as GameButton).SetNull();
                    //    }
                    //active = !active;  dla 2 graczy
                    //senderB.Enabled = false;
                    CheckWin(); 
                           
                   if (++Moves > 8) active = false;
                   if (active)     
                   {
                       int kh = 0, nr = 0, ns = 0;
                       for (int r = 0; r < 3; r++)
                           for (int s = 0; s < 3; s++)
                               if (gameButtons[r, s].IsClear)
                               { //sprawdzamy w kolumnie
                                   if (kh != 2)
                                   {
                                       kh = 0; for (int rr = 0; rr < 3; rr++)
                                           if (gameButtons[rr, s].IsZero)
                                           {
                                               kh++;
                                               if (kh == 2) { ns = s; nr = r; }
                                           }
                                   }
                                   //sprawdzamy w rzedzie
                                   if (kh != 2)
                                   {
                                       kh = 0;
                                       for (int ss = 0; ss < 3; ss++)
                                           if (gameButtons[r, ss].IsZero)
                                           {
                                               kh++;
                                               if (kh == 2) { ns = s; nr = r; }
                                           }
                                   }
                                   //sprawdzamy po przekatnej 1
                                   if ((kh != 2) && (r == s))
                                   {
                                       kh = 0;
                                       for (int ss = 0; ss < 3; ss++)
                                           if (gameButtons[ss, ss].IsZero)
                                           {
                                               kh++;
                                               if (kh == 2) { ns = s; nr = r; }
                                           }
                                   }
                                    //sprawdzamy po przekatnej 2
                                    if ((kh != 2) && (r == 2 - s))
                                   {
                                       kh = 0;
                                       for (int ss = 0; ss < 3; ss++)
                                           if (gameButtons[ss, 2 - ss].IsZero)
                                           {
                                               kh++;
                                               if (kh == 2) { ns = s; nr = r; }
                                           }
                                   }
                               }
                       if (kh != 2) 
                           for (int r = 0; r < 3; r++)
                               for (int s = 0; s < 3; s++)
                                   if (gameButtons[r, s].IsClear)
                                   { //sprawdzamy kolumne
                                       if (kh != 2)
                                       {
                                           kh = 0;
                                           for (int rr = 0; rr < 3; rr++)
                                               if (gameButtons[rr, s].IsCross)
                                               {
                                                   kh++;
                                                   if (kh == 2) { ns = s; nr = r; }
                                               }
                                       }
                                        //sprawdzamy rzad
                                        if (kh != 2)
                                       {
                                           kh = 0;
                                           for (int ss = 0; ss < 3; ss++)
                                               if (gameButtons[r, ss].IsCross)
                                               {
                                                   kh++;
                                                   if (kh == 2) { ns = s; nr = r; }
                                               }
                                       }
                                        //sprawdzamy po przekatnej 1

                                        if ((kh != 2) && (r == s))
                                       {
                                           kh = 0;
                                           for (int ss = 0; ss < 3; ss++)
                                               if (gameButtons[ss, ss].IsCross)
                                               {
                                                   kh++;
                                                   if (kh == 2) { ns = s; nr = r; }
                                               }
                                       }
                                        //sprawdzamy po przekatnej 2

                                        if ((kh != 2) && (r == 2 - s))
                                       {
                                           kh = 0;
                                           for (int ss = 0; ss < 3; ss++)
                                               if (gameButtons[ss, 2 - ss].IsCross)
                                               {
                                                   kh++;
                                                   if (kh == 2) { ns = s; nr = r; }
                                               }
                                       }
                                   }
                       if (kh != 2)  
                                     //randomowy krok
                       {
                           Random RN = new Random();
                           do { nr = RN.Next(3); ns = RN.Next(3); }
                           while (!gameButtons[nr, ns].IsClear);
                       }
                       gameButtons[nr, ns].SetNull(); Moves++;
                       CheckWin(); return;
                   }
                }
        }

    }
}



