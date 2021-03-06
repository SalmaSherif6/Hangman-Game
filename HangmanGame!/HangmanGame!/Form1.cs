using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Media;
namespace hang
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string[] fileWords;
        string randomWord;
        string randomWord2;
        string level;
        string category;

        int wordIndex;
        int wrongGuesses = 0;
        int wrongGuesses1 = 0;
        int wrongGuesses2 = 0;
        int score = 80;


        

        ToolTip t1 = new ToolTip();
        SoundPlayer sound;

        void One_Player_Cat(string[] fileWords)
        {
            //To Get Random Word
            fileWords = File.ReadAllLines(category + " " + level + ".txt");
            //MessageBox.Show("category is: " + category + "   " + level);
            Random random = new Random();
            wordIndex = random.Next(0, fileWords.Length - 1);
            randomWord = fileWords[wordIndex];
            //MessageBox.Show("Random: " + randomWord);

            //To Show "-"
            for (int i = 0; i < randomWord.Length; i++)
                if (randomWord[i] == ' ')
                    OnePlayerTextBox.Text += ' ';
                else
                    OnePlayerTextBox.Text += '-';
            OnePlayerShowCat.Text = Categories.Text;
        }

        void Two_Players_Cat(string[] fileWords)
        {
            //To Get Random Word
            fileWords = File.ReadAllLines(category + " " + level + ".txt");
            Random random = new Random();
            wordIndex = random.Next(0, fileWords.Length - 1);
            randomWord = fileWords[wordIndex];

            //To Show "-" for P1;
            for (int i = 0; i < randomWord.Length; i++)
                if (randomWord[i] == ' ')
                    PlayerOneTextBox.Text += ' ';
                else
                    PlayerOneTextBox.Text += '-';
            PlayerOneShowCat.Text = Categories.Text;

            //To Get Random Word for P2;
            wordIndex = random.Next(0, fileWords.Length - 1);
            randomWord2 = fileWords[wordIndex];

            //To Show "-" for P2;
            for (int i = 0; i < randomWord2.Length; i++)
                if (randomWord2[i] == ' ')
                    PlayerTwoTextBox.Text += ' ';
                else
                    PlayerTwoTextBox.Text += '-';
            PlayerTwoShowCat.Text = Categories.Text;
        }

        void Congrats()
        {
            MyHangman.SelectedTab = FinalPage;
            ResultTextBox.Text = "YOU WIN";
            WordTextBox.Text = "WORD: " + randomWord;
            SecondWordTextBox.Text = "SCORE: " + score;
            EmotionalPicture.Image = Image.FromFile("celebrate.gif");
            sound = new SoundPlayer("firework.wav");
            sound.Play();
        }

///////////////////////////********************************/////////////////////////////////////////////////////////////////
        private void Levels_SelectedIndexChanged(object sender, EventArgs e)
        {
            level = Levels.Text;
        }

        private void StartGameButton_Click(object sender, EventArgs e)
        {
            //again:
            //    if (category == null || level == null)
            //    {
            //        //MessageBox.Show("You must choose a level and a category.");
            //        goto again;
            //    }
                if (Players.Text == "ONE PLAYER")
                {
                    One_Player_Cat(fileWords);
                    MyHangman.SelectedTab = OnePlayerPage;
                    
                }
                else if (Players.Text == "TWO PLAYERS")
                {
                    Two_Players_Cat(fileWords);
                    MyHangman.SelectedTab = PlayerOnePage;
                    
            }
        }

        private void LettersButtons_Click(object sender, EventArgs e)
        {

            Button letter = (Button)sender;
            SoundPlayer buttonsound = new SoundPlayer("butttonsound.wav");
            buttonsound.Play();
            letter.Enabled = false;
            string test; //Test the word

            //To Check Whether Your Guess is Right or Wrong
            test = OnePlayerTextBox.Text;
            for (int i = 0; i < randomWord.Length; i++)
            {
                if (letter.Text[0] == randomWord[i])
                {
                    OnePlayerTextBox.Text = OnePlayerTextBox.Text.Remove(i, 1).Insert(i, letter.Text);

                    //If Right
                    if (OnePlayerTextBox.Text == randomWord)
                        Congrats();
                }
            }

            //Wrong Guess
            if (test == OnePlayerTextBox.Text)
            {
                wrongGuesses++;
                score -= 10;
                ScoreLabel.Text = "SCORE: " + score;

                switch (wrongGuesses)
                {
                    case 1:
                        HangmanPictures.Image = Image.FromFile("hang1.jpg");
                        break;
                    case 2:
                        HangmanPictures.Image = Image.FromFile("hang2.jpg");
                        break;
                    case 3:
                        HangmanPictures.Image = Image.FromFile("hang3.jpg");
                        break;
                    case 4:
                        HangmanPictures.Image = Image.FromFile("hang4.jpg");
                        break;
                    case 5:
                        HangmanPictures.Image = Image.FromFile("hang5.jpg");
                        break;
                    case 6:
                        HangmanPictures.Image = Image.FromFile("hang6.jpg");
                        break;
                    case 7:
                        HangmanPictures.Image = Image.FromFile("hang7.jpg");
                        break;
                }

            }

            //If Wrong
            if (wrongGuesses == 7)
            {
                MyHangman.SelectedTab = FinalPage;
                ResultTextBox.Text = "YOU LOSE";
                WordTextBox.Text = "WORD: " + randomWord;
                SecondWordTextBox.Text = "SCORE: " + score;
                EmotionalPicture.Image = Image.FromFile("wrong!.gif");
                sound = new SoundPlayer("Hanging.wav");
                sound.Play();
            }
        }

        private void HintButton_Click(object sender, EventArgs e)
        {
            SoundPlayer buttonsound = new SoundPlayer("butttonsound.wav");
            buttonsound.Play();
            Button Hint = (Button)sender;
            Hint.Enabled = false;
            Random random = new Random();
            int hint;

        Found:
            hint = random.Next(randomWord.Length);

            if (OnePlayerTextBox.Text[hint] == '-')
            {
                OnePlayerTextBox.Text = OnePlayerTextBox.Text.Remove(hint, 1).Insert(hint, randomWord[hint].ToString());
                score -= 10;
                ScoreLabel.Text = "Score: " + score;
                //MessageBox.Show("hint is: " + randomWord[hint]);
                //for (int i = 0; i < randomWord.Length; i++)
                //{
                    //char x = OnePlayerTextBox.Text[i];
                    //MessageBox.Show("" + x);
                    //MessageBox.Show(""+OnePlayerTextBox.Text[i]);
                    //if (x == randomWord[hint])
                    //{
                      //  x = x.Remove(hint, 1).Insert(hint, randomWord[hint]);
                    //}
                //}
                if (OnePlayerTextBox.Text == randomWord)
                    Congrats();
            }
            else
                goto Found;
        }

        private void PlayerOneLetters_Click(object sender, EventArgs e)
        {
            SoundPlayer soundbutton = new SoundPlayer("butttonsound.wav");
            soundbutton.Play();
            Button letter = (Button)sender;
            letter.Enabled = false;
            string test;

            //To Check Whether Your Guess is Right or Wrong
            test = PlayerOneTextBox.Text;
            for (int i = 0; i < randomWord.Length; i++)
            {
                if (letter.Text[0] == randomWord[i])
                {
                    PlayerOneTextBox.Text = PlayerOneTextBox.Text.Remove(i, 1).Insert(i, letter.Text);

                    //If Right
                    if (PlayerOneTextBox.Text == randomWord)
                    {
                        MyHangman.SelectedTab = FinalPage;
                        ResultTextBox.Text = "PLAYER ONE WINS";
                        WordTextBox.Text = "1ST WORD: " + randomWord;
                        SecondWordTextBox.Text = "2ND WORD: " + randomWord2;
                        EmotionalPicture.Image = Image.FromFile("celebrate.gif");
                        sound = new SoundPlayer("firework.wav");
                        sound.Play();
                    }
                }
            }

            //Wrong Guess
            if (test == PlayerOneTextBox.Text)
            {
                wrongGuesses1++;
                switch (wrongGuesses1)
                {
                    case 1:
                        PlayerOnePictures.Image = Image.FromFile("hang1.jpg");
                        break;
                    case 2:
                        PlayerOnePictures.Image = Image.FromFile("hang2.jpg");
                        break;
                    case 3:
                        PlayerOnePictures.Image = Image.FromFile("hang3.jpg");
                        break;
                    case 4:
                        PlayerOnePictures.Image = Image.FromFile("hang4.jpg");
                        break;
                    case 5:
                        PlayerOnePictures.Image = Image.FromFile("hang5.jpg");
                        break;
                    case 6:
                        PlayerOnePictures.Image = Image.FromFile("hang6.jpg");
                        break;
                    case 7:
                        PlayerOnePictures.Image = Image.FromFile("hang7.jpg");
                        break;
                }
                MyHangman.SelectedTab = PlayerTwoPage;
            }
        }

        private void PlayerOneHintButton_Click(object sender, EventArgs e)
        {
            SoundPlayer soundbutton = new SoundPlayer("butttonsound.wav");
            soundbutton.Play();
            Button Hint = (Button)sender;
            Hint.Enabled = false;
            Random random = new Random();
            int hint;

        Found:
            hint = random.Next(randomWord.Length);

            if (PlayerOneTextBox.Text[hint] == '-')
            {
                PlayerOneTextBox.Text = PlayerOneTextBox.Text.Remove(hint, 1).Insert(hint, randomWord[hint].ToString());

                if (PlayerOneTextBox.Text == randomWord)
                {
                    MyHangman.SelectedTab = FinalPage;
                    ResultTextBox.Text = "PLAYER ONE WINS";
                    WordTextBox.Text = "1ST WORD: " + randomWord;
                    SecondWordTextBox.Text = "2ND WORD: " + randomWord2;
                    EmotionalPicture.Image = Image.FromFile("celebrate.gif");
                    sound = new SoundPlayer("firework.wav");
                    sound.Play();
                }
            }
            else
                goto Found;
        }

        private void PlayerTwoLetters_Click(object sender, EventArgs e)
        {
            SoundPlayer buttonsound = new SoundPlayer("butttonsound.wav");
            buttonsound.Play();
            Button letter = (Button)sender;
            letter.Enabled = false;
            string test;

            //To Check Whether Your Guess is Right or Wrong
            test = PlayerTwoTextBox.Text;
            for (int i = 0; i < randomWord2.Length; i++)
            {
                if (letter.Text[0] == randomWord2[i])
                {
                    PlayerTwoTextBox.Text = PlayerTwoTextBox.Text.Remove(i, 1).Insert(i, letter.Text);

                    //If Right
                    if (PlayerTwoTextBox.Text == randomWord2)
                    {
                        MyHangman.SelectedTab = FinalPage;
                        ResultTextBox.Text = "PLAYER TWO WINS";
                        WordTextBox.Text = "1ST WORD: " + randomWord;
                        SecondWordTextBox.Text = "2ND WORD: " + randomWord2;
                        EmotionalPicture.Image = Image.FromFile("celebrate.gif");
                        sound = new SoundPlayer("firework.wav");
                        sound.Play();
                    }
                }
            }

            //Wrong Guess
            if (test == PlayerTwoTextBox.Text)
            {
                wrongGuesses2++;
                switch (wrongGuesses2)
                {
                    case 1:
                        PlayerTwoPictures.Image = Image.FromFile("hang1.jpg");
                        break;
                    case 2:
                        PlayerTwoPictures.Image = Image.FromFile("hang2.jpg");
                        break;
                    case 3:
                        PlayerTwoPictures.Image = Image.FromFile("hang3.jpg");
                        break;
                    case 4:
                        PlayerTwoPictures.Image = Image.FromFile("hang4.jpg");
                        break;
                    case 5:
                        PlayerTwoPictures.Image = Image.FromFile("hang5.jpg");
                        break;
                    case 6:
                        PlayerTwoPictures.Image = Image.FromFile("hang6.jpg");
                        break;
                    case 7:
                        PlayerTwoPictures.Image = Image.FromFile("hang7.jpg");
                        break;
                }

                if (wrongGuesses2 < 7)
                {
                    MyHangman.SelectedTab = PlayerOnePage;
                }

                //DRAW CASE
                else if (wrongGuesses2 == 7)
                {
                    MyHangman.SelectedTab = FinalPage;
                    ResultTextBox.Text = "DRAW!";
                    WordTextBox.Text = "1ST WORD: " + randomWord;
                    SecondWordTextBox.Text = "2ND WORD: " + randomWord2;
                    EmotionalPicture.Image = Image.FromFile("unnamed.gif");
                }
            }
        }

        private void PlayerTwoHintButton_Click(object sender, EventArgs e)
        {
            SoundPlayer buttonsound = new SoundPlayer("butttonsound.wav");
            buttonsound.Play();
            Button Hint = (Button)sender;
            Hint.Enabled = false;
            Random random = new Random();
            int hint;

        Found:
            hint = random.Next(randomWord2.Length);

            if (PlayerTwoTextBox.Text[hint] == '-')
            {
                PlayerTwoTextBox.Text = PlayerTwoTextBox.Text.Remove(hint, 1).Insert(hint, randomWord2[hint].ToString());

                if (PlayerTwoTextBox.Text == randomWord2)
                {
                    MyHangman.SelectedTab = FinalPage;
                    ResultTextBox.Text = "PLAYER TWO WINS";
                    WordTextBox.Text = "1ST WORD: " + randomWord;
                    SecondWordTextBox.Text = "2ND WORD: " + randomWord2;
                    EmotionalPicture.Image = Image.FromFile("celebrate.gif");
                    sound = new SoundPlayer("firework.wav");
                    sound.Play();
                }
            }
            else
                goto Found;
        }

        private void NextPuzzleButton_Click(object sender, EventArgs e)
        {
            Application.Restart();
            Environment.Exit(0);
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Thread.Sleep(500);
            Application.Exit();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            MyHangman.SelectedTab = StartPage;
        }

        private void Players_SelectedIndexChanged(object sender, EventArgs e)
        {
            MyHangman.SelectedTab = ChoicePage;
        }

        private void button36_MouseHover(object sender, EventArgs e)
        {
            t1.Show("Using the hint will make your score decreased by 10 \n You have only one hint", HintButton);
            t1.Show("Using the hint will make your score decreased by 10 \n you have only one hint", PlayerOneHintButton);
            t1.Show("Using the hint will make your score decreased by 10 \n you have only one hint", PlayerTwoHintButton);
        }

        private void Categories_SelectedIndexChanged(object sender, EventArgs e)
        {
            category = Categories.Text;
        }

        private void HangmanPicture_Click(object sender, EventArgs e)
        {

        }
    }
}