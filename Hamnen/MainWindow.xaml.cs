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

namespace Hamnen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int spaces = 32;
        int boatsPerDay = 5;

        bool Position = true;
        bool Weight = false;
        bool Speed = false;
        bool Duration = false;

        List<Pier> Piers = new List<Pier>();
        List<Boat> IncomingBoats = new List<Boat>();
        List<Boat> LoadBoat = new List<Boat>();
        List<Boat> LoadBoat2 = new List<Boat>();
        List<Boat> RejectedShips = new List<Boat>();

        public MainWindow()
        {
            InitializeComponent();
            GeneratePiers(Piers, spaces);
            LoadFile(@"C:\Users\Max\source\repos\CSharp_app\CSharp_app\bin\Debug\Pier1.text", LoadBoat, 0);
            CopyFileToPier(Piers, LoadBoat, 0);
            LoadFile(@"C:\Users\Max\source\repos\CSharp_app\CSharp_app\bin\Debug\Pier2.text", LoadBoat2, 1);
            CopyFileToPier(Piers, LoadBoat2, 1);
            UpdateLB3(RejectedShips);
            InfoBox(Piers);
            UpdateInterface(Piers);
        }

        // Nästa dag knappen
        private void ButtonNextDay_Click(object sender, RoutedEventArgs e)
        {
            DurationAndLeaving(Piers);
            SpawnBoat(IncomingBoats, boatsPerDay);
            AddBoatToPier(Piers, IncomingBoats, RejectedShips);
            SaveToFile(Piers);
            LoadFile(@"C:\Users\Max\source\repos\CSharp_app\CSharp_app\bin\Debug\Pier1.text", LoadBoat, 0);
            LoadFile(@"C:\Users\Max\source\repos\CSharp_app\CSharp_app\bin\Debug\Pier2.text", LoadBoat2, 1);
            UpdateLB3(RejectedShips);
            UpdateInterface(Piers);
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------------

        #region Copy and load
        // Kopierar över värden i textfilen som sedan ges till pirerna
        private void CopyFileToPier(List<Pier> piers, List<Boat> loadBoat, int pierNum)
        {
            foreach (Boat x in loadBoat)
            {
                if (x.ID.StartsWith("R"))
                {
                    piers[pierNum].BoatsInPier.Add(new Rowboat(x.ID, x.Weight, x.Speed, x.Duration, x.Position, x.XFactor));
                    piers[pierNum].Space[x.Position] += 0.5;
                }
                if (x.ID.StartsWith("M"))
                {
                    piers[pierNum].BoatsInPier.Add(new Motorboat(x.ID, x.Weight, x.Speed, x.Duration, x.Position, x.XFactor));
                    piers[pierNum].Space[x.Position] = 1;
                }
                if (x.ID.StartsWith("S"))
                {
                    piers[pierNum].BoatsInPier.Add(new Sailboat(x.ID, x.Weight, x.Speed, x.Duration, x.Position, x.XFactor));
                    piers[pierNum].Space[x.Position] = 1;
                    piers[pierNum].Space[x.Position + 1] = 1;
                }
                if (x.ID.StartsWith("C"))
                {
                    piers[pierNum].BoatsInPier.Add(new Catamaran(x.ID, x.Weight, x.Speed, x.Duration, x.Position, x.XFactor));
                    piers[pierNum].Space[x.Position] = 1;
                    piers[pierNum].Space[x.Position + 1] = 1;
                    piers[pierNum].Space[x.Position + 2] = 1;
                }
                if (x.ID.StartsWith("F"))
                {
                    piers[pierNum].BoatsInPier.Add(new Freightship(x.ID, x.Weight, x.Speed, x.Duration, x.Position, x.XFactor));
                    piers[pierNum].Space[x.Position] = 1;
                    piers[pierNum].Space[x.Position + 1] = 1;
                    piers[pierNum].Space[x.Position + 2] = 1;
                    piers[pierNum].Space[x.Position + 3] = 1;
                }
            }

        }
        //----------------------------------------------------------------------------------------------------------------------------------------------------


        // Laddar in textfilens info till programmet
        private void LoadFile(string path, List<Boat> loadBoat, int num)
        {
            if (num == 0) LoadBoat.Clear();
            if (num == 1) LoadBoat2.Clear();

            string[] BoatsPier1List = File.ReadAllLines(path);
            if(num == 0)listBoxPier1B.Items.Clear();
            if (num == 1) ListBoxPier2.Items.Clear();
            
            for (int i = 0; i < BoatsPier1List.Length; i++)
            {
                string[] tempBoatString = BoatsPier1List[i].Split(',');

                if(tempBoatString[0].StartsWith("R")) loadBoat.Add(new Rowboat(tempBoatString[0], Convert.ToInt32(tempBoatString[1]), Convert.ToInt32(tempBoatString[2]),
                    Convert.ToInt32(tempBoatString[3]), Convert.ToInt32(tempBoatString[4]), Convert.ToInt32(tempBoatString[5])));
                if (tempBoatString[0].StartsWith("M")) loadBoat.Add(new Motorboat(tempBoatString[0], Convert.ToInt32(tempBoatString[1]), Convert.ToInt32(tempBoatString[2]),
                     Convert.ToInt32(tempBoatString[3]), Convert.ToInt32(tempBoatString[4]), Convert.ToInt32(tempBoatString[5])));
                if (tempBoatString[0].StartsWith("S")) loadBoat.Add(new Sailboat(tempBoatString[0], Convert.ToInt32(tempBoatString[1]), Convert.ToInt32(tempBoatString[2]),
                     Convert.ToInt32(tempBoatString[3]), Convert.ToInt32(tempBoatString[4]), Convert.ToInt32(tempBoatString[5])));
                if (tempBoatString[0].StartsWith("C")) loadBoat.Add(new Catamaran(tempBoatString[0], Convert.ToInt32(tempBoatString[1]), Convert.ToInt32(tempBoatString[2]),
                     Convert.ToInt32(tempBoatString[3]), Convert.ToInt32(tempBoatString[4]), Convert.ToInt32(tempBoatString[5])));
                if (tempBoatString[0].StartsWith("F")) loadBoat.Add(new Freightship(tempBoatString[0], Convert.ToInt32(tempBoatString[1]), Convert.ToInt32(tempBoatString[2]),
                     Convert.ToInt32(tempBoatString[3]), Convert.ToInt32(tempBoatString[4]), Convert.ToInt32(tempBoatString[5])));
            }
            foreach (Boat x in loadBoat)
            {
                if (x.ID.StartsWith("R"))
                {
                    x.Path = @"C:\Users\Max\Pictures\rowboat.png";
                    x.xAdd = $"Seats: ";
                    x.posAdd = $"     ";
                }
                if (x.ID.StartsWith("M"))
                {
                    x.Path = @"C:\Users\Max\Pictures\motorboat.png";
                    x.xAdd = $"Horsepower: ";
                    x.posAdd = $"     ";
                }
                if (x.ID.StartsWith("S"))
                {
                    x.Path = @"C:\Users\Max\Pictures\sailboat.png";
                    x.xAdd = $"Length: ";
                    x.posAdd = $" - {x.Position + 2}";
                }
                if (x.ID.StartsWith("C"))
                {
                    x.Path = @"C:\Users\Max\Pictures\catamaran.png";
                    x.xAdd = $"Beds: ";
                    x.posAdd = $" - {x.Position + 3}";
                }
                if (x.ID.StartsWith("F"))
                {
                    x.Path = @"C:\Users\Max\Pictures\freightship.png";
                    x.xAdd = $"Containers: ";
                    x.posAdd = $" - {x.Position + 4}";
                }

            }
            OrderBy(loadBoat, num);
        }
        #endregion Copy and load
        //----------------------------------------------------------------------------------------------------------------------------------------------------

        #region Sorting
        // Sorteringsuttryck
        private void OrderBy(List<Boat> loadBoat, int num)
        {
            var q1 = loadBoat.OrderBy(boat => boat.Position)
                .Select(boat => new { Position = (boat.Position + 1), ID = boat.ID, kilos = boat.Weight, speed = (boat.Speed * 1.85), dur = boat.Duration, xfactor = boat.XFactor, path = boat.Path, xadd = boat.xAdd, posadd = boat.posAdd });
            var q2 = loadBoat.OrderBy(boat => boat.Weight)
                .Select(boat => new { Position = (boat.Position + 1), ID = boat.ID, kilos = boat.Weight, speed = (boat.Speed * 1.85), dur = boat.Duration, xfactor = boat.XFactor, path = boat.Path, xadd = boat.xAdd, posadd = boat.posAdd });
            var q3 = loadBoat.OrderBy(boat => boat.Speed)
                .Select(boat => new { Position = (boat.Position + 1), ID = boat.ID, kilos = boat.Weight, speed = (boat.Speed * 1.85), dur = boat.Duration, xfactor = boat.XFactor, path = boat.Path, xadd = boat.xAdd, posadd = boat.posAdd });
            var q4 = loadBoat.OrderBy(boat => boat.Duration)
                .Select(boat => new { Position = (boat.Position + 1), ID = boat.ID, kilos = boat.Weight, speed = (boat.Speed * 1.85), dur = boat.Duration, xfactor = boat.XFactor, path = boat.Path, xadd = boat.xAdd, posadd = boat.posAdd });

            if (Position)
            {
                foreach (var x in q1)
                {
                    if (num == 0) listBoxP1.ItemsSource = q1;
                    if (num == 1) listBoxP2.ItemsSource = q1;
                }
            }
            else if (Weight)
            {
                foreach (var x in q2)
                {
                    if (num == 0) listBoxP1.ItemsSource = q2;
                    if (num == 1) listBoxP2.ItemsSource = q2;
                }
            }
            else if (Speed)
            {
                foreach (var x in q3)
                {
                    if (num == 0) listBoxP1.ItemsSource = q3;
                    if (num == 1) listBoxP2.ItemsSource = q3;
                }
            }
            else if (Duration)
            {
                foreach (var x in q4)
                {
                    if (num == 0) listBoxP1.ItemsSource = q4;
                    if (num == 1) listBoxP2.ItemsSource = q4;
                }
            }
        }
        #endregion Sorting
        //----------------------------------------------------------------------------------------------------------------------------------------------------

        #region Saving
        //Spara info i textfil
        private void SaveToFile(List<Pier> piers)
        {
            AddBoatToFile(piers, 0, @"C:\Users\Max\source\repos\CSharp_app\CSharp_app\bin\Debug\Pier1.text");
            AddBoatToFile(piers, 1, @"C:\Users\Max\source\repos\CSharp_app\CSharp_app\bin\Debug\Pier2.text");
        }

        private void AddBoatToFile(List<Pier> pier,int num, string filePath)
        {
            using (StreamWriter saver1 = new StreamWriter(filePath, false))
            {
                foreach (Boat x in pier[num].BoatsInPier)
                {
                    saver1.WriteLine($"{x.ID},{x.Weight},{x.Speed},{x.Duration},{x.Position},{x.XFactor}");
                }
            }
        }
        #endregion Saving
        //----------------------------------------------------------------------------------------------------------------------------------------------------

        #region Info
        // Uppdatera alla siffror
        private void UpdateInterface(List<Pier> piers)
        {
            ListBoxPier2.Items.Clear();
            listBoxPier1B.Items.Clear();

            int counter = 0;

            // Listbox 1
            foreach (Boat x in piers[0].BoatsInPier)
            {
                counter++;
            }

            // Ställer in vad man ser
            for (int i = 0; i < piers[0].Space.Length; i++)
            {
                listBoxPier1B.Items.Add($"{i + 1}: {piers[0].Space[i]}");
            }

            // Listbox2
            counter = 0;

            foreach (Boat x in piers[1].BoatsInPier)
            {
                counter++;
            }

            for (int i = 0; i < piers[1].Space.Length; i++)
            {
                ListBoxPier2.Items.Add($"{i + 1}: {piers[1].Space[i]}");
            }

            // Räknar hur många platser som tas upp
            double counter2 = 0;
            for (int i = 0; i < piers[0].Space.Length; i++)
            {
                counter2 += piers[0].Space[i];
            }
            label2.Content = $"Slots taken: {counter2}";

            counter2 = 0;
            for (int i = 0; i < piers[1].Space.Length; i++)
            {
                counter2 += piers[1].Space[i];
            }
            label3.Content = $"Slots taken: {counter2}";

            InfoBox(piers);
        }
        
        //----------------------------------------------------------------------------------------------------------------------------------------------------


        // Uppdaterar infoboxen
        private void InfoBox(List<Pier> piers)
        {
            int R = 0, M = 0, S = 0, K = 0, F = 0, TWeight = 0, TShip = 0, TotalSpeed = 0, FreeSlots = 0;
            double ASpeed = 0, AWeight = 0;
            listBoxInfoPier1.Items.Clear();

            foreach (Pier x in piers)
            {
                for (int i = 0; i < x.Space.Length; i++)
                {
                    if (x.Space[i] == 0) FreeSlots++;
                }

                foreach (Boat y in x.BoatsInPier)
                {
                    if (y.ID.StartsWith("R"))
                    {
                        R++;
                        TShip++;
                        TWeight += y.Weight;
                        TotalSpeed += y.Speed;
                    }
                    if (y.ID.StartsWith("M"))
                    {
                        M++;
                        TShip++;
                        TWeight += y.Weight;
                        TotalSpeed += y.Speed;
                    }
                    if (y.ID.StartsWith("S"))
                    {
                        S++;
                        TShip++;
                        TWeight += y.Weight;
                        TotalSpeed += y.Speed;
                    }
                    if (y.ID.StartsWith("C"))
                    {
                        K++;
                        TShip++;
                        TWeight += y.Weight;
                        TotalSpeed += y.Speed;
                    }
                    if (y.ID.StartsWith("F"))
                    {
                        F++;
                        TShip++;
                        TWeight += y.Weight;
                        TotalSpeed += y.Speed;
                    }
                } 
            }
            try
            {
                ASpeed = (TotalSpeed * 1.85) / TShip;
                ASpeed = Math.Round(ASpeed, 2);
                AWeight = TWeight / TShip;
            }
            catch
            {
                Console.WriteLine("Divide by zero");
            }

            listBoxInfoPier1.Items.Add($"Rowboat {R} Motorboats: {M} Sailboats: {S} Catamarans: {K} Freightship: {F} \nTotal Ships: {TShip} \n" +
                $"Average speed km/h: {ASpeed} \nAverage weight: {AWeight}\nFree slots: {FreeSlots}");
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------------

        //Uppdaterar Rejected ships listan
        private void UpdateLB3(List<Boat> RejectedShips)
        {
            listBox3.Items.Clear();
            int rejected = 0;

            foreach (Boat x in RejectedShips)
            {
                listBox3.Items.Add($"ID : {x.ID} Weight: {x.Weight} Speed: {x.Speed}");
                rejected++;
            }

            LabelRejectedShips.Content = $"Rejected ships: {rejected}";
        }
        #endregion Info
        //----------------------------------------------------------------------------------------------------------------------------------------------------

        #region At start
        // Skapar pirerna
        private void GeneratePiers(List<Pier> pierList, int spaces)
        {
            pierList.Add(new Pier());
            pierList.Add(new Pier());

            foreach (Pier x in pierList)
            {
                x.Space = new double[spaces];

                // Försäkrar att alla värden i listan space är 0
                for (int i = 0; i < x.Space.Length; i++)
                {
                    x.Space[i] = 0;
                }
            }

            //Sätter bilden på piren
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(@"C:\Users\Max\Pictures\pir2.png");
            bitmap.EndInit();
            imagePier1.Source = bitmap;
            imagePier2.Source = imagePier1.Source;
            BitmapImage bitmap2 = new BitmapImage();
            bitmap2.BeginInit();
            bitmap2.UriSource = new Uri(@"C:\Users\Max\Pictures\ocean3.png");
            bitmap2.EndInit();
            imageOcean.Source = bitmap2;
        }
        #endregion At start
        //----------------------------------------------------------------------------------------------------------------------------------------------------

        #region Add and remove boat
        // Skapar lista med båtar som ska lägga an
        private static void SpawnBoat(List<Boat> boats, int boatsperday)
        {
            Random rand = new Random();

            boats.Clear();

            for (int i = 0; i < boatsperday; i++)
            {
                // Bestäm ID på båten
                string charSallad = "";

                for (int j = 0; j < 3; j++)
                {
                    char randchar = (char)('a' + rand.Next(0, 26));
                    charSallad += randchar;
                }

                int dice = rand.Next(0, 4 + 1);

                // Skapa båt
                if (dice == 0) boats.Add(new Rowboat("R-" + charSallad.ToUpper(), rand.Next(100, 300), rand.Next(0, 3), 1, 0, rand.Next(1, 6)));
                else if (dice == 1) boats.Add(new Motorboat("M-" + charSallad.ToUpper(), rand.Next(200, 3000), rand.Next(0, 60), 3, 0, rand.Next(10, 1000)));
                else if (dice == 2) boats.Add(new Sailboat("S-" + charSallad.ToUpper(), rand.Next(800, 6000), rand.Next(0, 12), 4, 0, rand.Next(10, 60)));
                else if (dice == 3) boats.Add(new Catamaran("C-" + charSallad.ToUpper(), rand.Next(1200, 8000), rand.Next(0, 12), 3, 0, rand.Next(1, 4)));
                else if (dice == 4) boats.Add(new Freightship("F-" + charSallad.ToUpper(), rand.Next(3000, 20000), rand.Next(0, 20), 6, 0, rand.Next(0, 500)));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------------


        // Lägger till båtarna i pirerna
        private static void AddBoatToPier(List<Pier> piers, List<Boat> incomingBoats, List<Boat> reject)
        {
            double countPier1 = 0, countPier2 = 0;
            
            // Väljer pir för båtarna
            foreach (Boat x in incomingBoats)
            {
                //Räknar antal lediga platser som finns
                for (int i = 0; i < piers[0].Space.Length; i++)
                {
                    countPier1 += piers[0].Space[i];
                }

                for (int i = 0; i < piers[1].Space.Length; i++)
                {
                    countPier2 += piers[1].Space[i];
                }

                if (countPier1 <= countPier2)
                {
                    piers[0].BoatsAtPier.Add(x);
                    PickAnEmptySpot(piers,piers[0], incomingBoats,reject);
                }
                else
                {
                    piers[1].BoatsAtPier.Add(x);
                    PickAnEmptySpot(piers, piers[1], incomingBoats, reject);
                }
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------------


        // Väljer plats för båt
        private static void PickAnEmptySpot(List<Pier> piers,Pier pier, List<Boat> incomingBoats, List<Boat> reject)
        {
            foreach (Pier x in piers)
            {
                foreach (Boat y in x.BoatsAtPier.ToList())
                {
                    bool leave = true;

                    if (y.ID.StartsWith("R"))
                    {
                        for (int i = 0; i < x.Space.Length; i++)
                        {
                            if (x.Space[i] == 0 || x.Space[i] == 0.5)
                            {
                                x.Space[i] += 0.5;
                                x.BoatsInPier.Add(new Rowboat(y.ID, y.Weight, y.Speed, y.Duration, i, y.XFactor));
                                x.BoatsAtPier.Remove(y);
                                leave = false;
                                break;
                            }
                        }
                    }
                    else if (y.ID.StartsWith("M"))
                    {
                        for (int i = 0; i < x.Space.Length; i++)
                        {
                            if (x.Space[i] == 0)
                            {
                                x.Space[i] = 1;
                                x.BoatsInPier.Add(new Motorboat(y.ID, y.Weight, y.Speed, y.Duration, i, y.XFactor));
                                x.BoatsAtPier.Remove(y);
                                leave = false;
                                break;
                            }
                        }
                    }
                    else if (y.ID.StartsWith("S"))
                    {
                        for (int i = 0; i < x.Space.Length - 1; i++)
                        {
                            if (x.Space[i] == 0 && x.Space[i + 1] == 0)
                            {
                                x.Space[i] = 1;
                                x.Space[i + 1] = 1;
                                x.BoatsInPier.Add(new Sailboat(y.ID, y.Weight, y.Speed, y.Duration, i, y.XFactor));
                                x.BoatsAtPier.Remove(y);
                                leave = false;
                                break;
                            }
                        }
                    }
                    else if (y.ID.StartsWith("C"))
                    {
                        for (int i = 0; i < x.Space.Length - 3; i++)
                        {
                            if (x.Space[i] == 0 && x.Space[i + 1] == 0 && x.Space[i + 2] == 0)
                            {
                                x.Space[i] = 1;
                                x.Space[i + 1] = 1;
                                x.Space[i + 2] = 1;
                                x.BoatsInPier.Add(new Catamaran(y.ID, y.Weight, y.Speed, y.Duration, i, y.XFactor));
                                x.BoatsAtPier.Remove(y);
                                leave = false;
                                break;
                            }
                        }
                    }
                    else if (y.ID.StartsWith("F"))
                    {
                        for (int i = x.Space.Length - 4; i >= 0; i--)
                        {
                            if (x.Space[i] == 0 && x.Space[i + 1] == 0 && x.Space[i + 2] == 0 && x.Space[i + 3] == 0)
                            {
                                x.Space[i] = 1;
                                x.Space[i + 1] = 1;
                                x.Space[i + 2] = 1;
                                x.Space[i + 3] = 1;
                                x.BoatsInPier.Add(new Freightship(y.ID, y.Weight, y.Speed, y.Duration, i, y.XFactor));
                                x.BoatsAtPier.Remove(y);
                                leave = false;
                                break;
                            }
                        }
                    }
                    // Båtar som inte hittar plats och läggs till i åka iväg listan
                    if (leave)
                    {
                        if (y.ID.StartsWith("R"))
                        {
                            reject.Add(new Rowboat(y.ID, y.Weight, y.Speed, y.Duration, y.Position, y.XFactor));
                            x.BoatsAtPier.Remove(y);
                        }
                        if (y.ID.StartsWith("M"))
                        {
                            reject.Add(new Motorboat(y.ID, y.Weight, y.Speed, y.Duration, y.Position, y.XFactor));
                            x.BoatsAtPier.Remove(y);
                        }
                        if (y.ID.StartsWith("S"))
                        {
                            reject.Add(new Sailboat(y.ID, y.Weight, y.Speed, y.Duration, y.Position, y.XFactor));
                            x.BoatsAtPier.Remove(y);
                        }
                        if (y.ID.StartsWith("C"))
                        {
                            reject.Add(new Catamaran(y.ID, y.Weight, y.Speed, y.Duration, y.Position, y.XFactor));
                            x.BoatsAtPier.Remove(y);
                        }
                        if (y.ID.StartsWith("F"))
                        {
                            reject.Add(new Freightship(y.ID, y.Weight, y.Speed, y.Duration, y.Position, y.XFactor));
                            x.BoatsAtPier.Remove(y);
                        }
                    }
                }
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------------


        // Räknar ned tiden för en båt samt låter dem lämna piren
        private static void DurationAndLeaving(List<Pier> piers)
        {
            foreach (Pier x in piers)
            {
                foreach (Boat y in x.BoatsInPier.ToList())
                {
                    y.Duration -= 1;

                    if (y.Duration == 0)
                    {
                        if (y.ID.StartsWith("R"))
                        {
                            x.Space[y.Position] -= 0.5;
                            x.BoatsInPier.Remove(y);
                        }
                        else if (y.ID.StartsWith("M"))
                        {
                            x.Space[y.Position] = 0;
                            x.BoatsInPier.Remove(y);
                        }
                        else if (y.ID.StartsWith("S"))
                        {
                            x.Space[y.Position] = 0;
                            x.Space[y.Position + 1] = 0;
                            x.BoatsInPier.Remove(y);
                        }
                        else if (y.ID.StartsWith("C"))
                        {
                            x.Space[y.Position] = 0;
                            x.Space[y.Position + 1] = 0;
                            x.Space[y.Position + 2] = 0;
                            x.BoatsInPier.Remove(y);
                        }
                        else if (y.ID.StartsWith("F"))
                        {
                            x.Space[y.Position] = 0;
                            x.Space[y.Position + 1] = 0;
                            x.Space[y.Position + 2] = 0;
                            x.Space[y.Position + 3] = 0;
                            x.BoatsInPier.Remove(y);
                        }
                    }
                }
            }
        }
        #endregion Add and remove boat
        //----------------------------------------------------------------------------------------------------------------------------------------------------

        #region Choices
        // Byta view i boxarna

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Position = true;
            Weight = false;
            Speed = false;
            Duration = false;
            listBoxPier1B.Items.Clear();
            ListBoxPier2.Items.Clear();
            OrderBy(LoadBoat, 0);
            OrderBy(LoadBoat2, 1);
            UpdateInterface(Piers);
        }

        private void Button_Copy_Click(object sender, RoutedEventArgs e)
        {
            Position = false;
            Weight = true;
            Speed = false;
            Duration = false;
            listBoxPier1B.Items.Clear();
            ListBoxPier2.Items.Clear();
            OrderBy(LoadBoat, 0);
            OrderBy(LoadBoat2, 1);
            UpdateInterface(Piers);
        }

        private void Button_Copy1_Click(object sender, RoutedEventArgs e)
        {
            Position = false;
            Weight = false;
            Speed = true;
            Duration = false;
            listBoxPier1B.Items.Clear();
            ListBoxPier2.Items.Clear();
            OrderBy(LoadBoat, 0);
            OrderBy(LoadBoat2, 1);
            UpdateInterface(Piers);
        }

        private void Button_Copy2_Click(object sender, RoutedEventArgs e)
        {
            Position = false;
            Weight = false;
            Speed = false;
            Duration = true;
            listBoxPier1B.Items.Clear();
            ListBoxPier2.Items.Clear();
            OrderBy(LoadBoat, 0);
            OrderBy(LoadBoat2, 1);
            UpdateInterface(Piers);
        }
        #endregion Choices
    }
}
