using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
 
namespace Dalverseny
{
    class Versenyzo
    {
        // mező

        private int rajtSz;
        private string neve;
        private string szak;
        private int pontSz;

        // konstruktor
        public Versenyzo(int rajtSz, string neve, string szak)
        {
            this.rajtSz = rajtSz;
            this.neve = neve;
            this.szak = szak;
        }

        // metódusok

        public void PontotKap(int pont)
        {
            pontSz += pont;
        }

        public override string ToString()
        {
            return rajtSz + "\t" + neve + "\t" + szak + "\t" + pontSz + " pont";
        }

        // tulajdonságok

        public int rajtSz
        {
            get { return rajtSz; }
        }

        public string neve
        {
            get { return neve; }
        }

        public string Szak
        {
            get { return szak; }
        }

        public int pontSz
        {
            get { return pontSz; }
        }

        //---------------------------


    }

    class VezerloOsztaly
    {
        private List<Versenyzo> versenyzok = new List<Versenyzo>();

        public void Start()
        {
            AdatBevitel();

            Kiiratas("\nRésztvevők:\n");
            Verseny();
            Kiiratas("\nEredményeik:\n");

            Keresesek();

            Nyertes();
            Sorrend();
        }

        public void AdatBevitel()
        {
            Versenyzo versenyzo;
            string neve, szak;
            int sorszam = 1;

            StreamReader olvasoCsatorna = new StreamReader("versenyzok.txt");

            while (!olvasoCsatorna.EndOfStream)
            {
                neve = olvasoCsatorna.ReadLine();
                szak = olvasoCsatorna.ReadLine();

                // versenyző példány létrehozása
                versenyzo = new Versenyzo(sorszam, neve, szak);

                // versenyző listához adása
                versenyzok.Add(versenyzo);


                sorszam++;
            }

            olvasoCsatorna.Close();
        }

        private void Kiiratas(string cim)
        {
            Console.WriteLine(cim);
            foreach (Versenyzo enekes in versenyzok)
            {
                Console.WriteLine(enekes);
            }
        }

        private int zsuriLetszam = 5;
        private int pontHatar = 10;


        private void Verseny()
        {
            Random rand = new Random();
            int pont;
            foreach (Versenyzo versenyzo in versenyzok)
            {
                // zsűri pontozása

                for (int i = 0; i < zsuriLetszam; i++)
                {
                    pont = rand.Next(pontHatar);
                    versenyzo.PontotKap(pont);
                }
            }
        }

        private void Nyertes()
        {
            // Kezdőérték

            int max = versenyzok[0].pontSz;

            // a max érték megállapítása

            foreach (Versenyzo enekes in versenyzok)
            {
                if (enekes.pontSz > max)
                {
                    max = enekes.pontSz;
                }
            }

            // legjobbak kiíratása

            Console.WriteLine("\nLegjobbak:\n");
            foreach (Versenyzo enekes in versenyzok)
            {
                if (enekes.pontSz == max)
                {
                    Console.WriteLine(enekes);
                }
            }
        }

        private void Sorrend()
        {
            //rendezés

            Versenyzo temp;

            for (int i = 0; i < versenyzok.Count() - 1; i++)
            {
                for (int j = 0; j < versenyzok.Count(); j++)
                {
                    if (versenyzok[i].pontSz < versenyzok[j].pontSz)
                    {
                        temp = versenyzok[i];
                        versenyzok[i] = versenyzok[j];
                        versenyzok[j] = temp;
                    }
                }
            }

            Kiiratas("\nEredménytábla\n");
        }

        private void Keresesek()
        {
            Console.WriteLine("\nA megadott szakhoz tartozó énekesek keresése\n");
            Console.Write("\nKeresel valakit? (i/n)");
            char valasz;
            while (!char.TryParse(Console.ReadLine(), out valasz))
            {
                Console.Write("1 karaktert írj! (i/n) ");
            }

            string szak;
            bool vanIlyen;

            while (valasz == 'i' || valasz == 'I')
            {
                Console.Write("Szak: ");
                szak = Console.ReadLine();
                vanIlyen = false;

                foreach (Versenyzo enek in versenyzok)
                {
                    if (enek.Szak == szak)
                    {
                        Console.WriteLine(enek);
                        vanIlyen = true;
                    }
                }

                if (!vanIlyen)
                {
                    Console.WriteLine("Erről a szakról egy ember sem idnult.");
                }

                Console.Write("\nKeresel még mindig valakit? (i/n)");
                valasz = char.Parse(Console.ReadLine());
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            new VezerloOsztaly().Start();

            Console.ReadKey();
        }
    }
}