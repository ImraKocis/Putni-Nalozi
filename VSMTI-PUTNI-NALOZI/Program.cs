using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Globalization;
using ConsoleTables;

/*
 |--------------------------|
 |  Izradio: Imra Kočiš     |
 |  Smjer: PIN-1            |
 |  jmbag: 0135256033       |
 |--------------------------|
 */

namespace VSMTI_PUTNI_NALOZI
{
    class Program
    {
        public static List<PutniNalog> SviPutniNalozi = new List<PutniNalog>(); 
        public static List<Zaposlenik> SviZaposlenici = new List<Zaposlenik>();
        public static List<TipVozila> SvaVozila = new List<TipVozila>();
        public static List<IskoristeniZaposlenici> SviIskoristeniZap = new List<IskoristeniZaposlenici>();
        

        public struct LogIn
        {
            public string ime;
            public string pass;

            public LogIn(string i, string p)
            {
                ime = i;
                pass = p;
            }
        }

        public struct TipVozila
        {
            public int ID;
            public string Naziv;

            public TipVozila(int id, string naziv)
            {
                ID = id;
                Naziv = naziv;
            }
        }        
        public struct Zaposlenik
        {
            public int id;
            public string imePrezime;            
            public Zaposlenik(int i, string ip)
            {
                id = i;
                imePrezime = ip;                
            }
        }
        public struct IskoristeniZaposlenici
        {
            public int ID;
            public string ImeIPrezime;
            public string OsobniID;

            public IskoristeniZaposlenici (int id, string imeip, string oib)
            {
                ID = id;
                ImeIPrezime = imeip;
                OsobniID = oib;
            }
        }
        public struct PutniNalog
        {
            public int IDPutnogNaloga;
            public string ImeIPrezime;
            public string VPrijevoza;
            public string Polaziste;
            public string Odrediste;
            public string DatumO;
            public string DatumP;
            public string Trajanje;

            public PutniNalog(int rbr, string ime, string pri, string pol, string odr, string d1, string d2, string tra)
            {
                IDPutnogNaloga = rbr;
                ImeIPrezime = ime;
                VPrijevoza = pri;
                Polaziste = pol;
                Odrediste = odr;
                DatumO = d1;
                DatumP = d2;
                Trajanje = tra;

            }
        }
        // Funkcija izbornik koja ispisuje izbornik i traži od korisnika unos 
        public static void Izbornik()
        {
            Console.WriteLine("1:\tISPIS SVIH PUTNIH NALOGA");
            Console.WriteLine("2:\tISPIS PUTNIH NALOGA SVIH ZAPOSLENIKA");
            Console.WriteLine("3:\tSTATISTIKA PRIJEVOZNIH SREDSTAVA");
            Console.WriteLine("4:\tSTATISTIKA POLAZIŠTA");
            Console.WriteLine("5:\tSTATISTIKA ODREDIŠTA");
            Console.WriteLine("6:\tDODAJ ZAPOSLENIKA");
            Console.WriteLine("7:\tIZRADI PURNI NALOG");
            Console.WriteLine("8:\tODJAVA");
            Console.WriteLine("\n\nX ZA IZLAZ IZ PROGRAMA\n");
            Console.WriteLine("\nUnesite broj koji se nalazi ispred zeljene funkcije.\n");
            int izbor = Convert.ToInt32(Console.ReadKey().Key);
            // X -> 88
            if (izbor == 88)
            {
                Console.WriteLine("\n***IZLAZ***");
                System.Threading.Thread.Sleep(2000);
                System.Environment.Exit(1);
            }
            int[] DozvoljeniUnosi = { 49, 50, 51, 52, 53, 54, 55, 88, 56 };
            while (!DozvoljeniUnosi.Contains(izbor))
            {                             
                if (izbor == 88)
                {
                    Console.WriteLine("\n\n***IZLAZ***");
                    System.Threading.Thread.Sleep(2000);
                    System.Environment.Exit(1);
                }
                Console.WriteLine("\nKrivi odabir");
                izbor = Convert.ToInt32(Console.ReadKey().Key);
            }

            switch (izbor)
            {
                case 49:
                    IspisSvihNaloga();
                    Povratak();                    
                    break;
                case 50:
                    IspisNalogaZaposlenika();
                    Povratak();
                    break;
                case 51:
                    StatistikaPrijevoznihSredstava();
                    Povratak();
                    break;
                case 52:
                    StatistikaPolazista();
                    Povratak();
                    break;
                case 53:
                    StatistikaOdredista();
                    Povratak();
                    break;
                case 54:
                    UpisZaposlenika();
                    Povratak();
                    break;
                case 55:
                    UpisNaloga();
                    Povratak();
                    break;
                case 56:
                    ProgramStart();
                    break;
                default:
                    break;
            }

        }
        // Funkcija Povratak ispisuje izbor i trazi od korisnika unos
        public static void Povratak()
        {
            Console.WriteLine("\nQ - Glavni izbornik");
            Console.WriteLine("\nX - Izlaz");
            // Q -> 81
            // X -> 88            
            int izbor = Convert.ToInt32(Console.ReadKey().Key);
            while (izbor != 81 && izbor != 88)
            {
                Console.WriteLine("\nNeispravan izbor!\n");
                izbor = Convert.ToInt32(Console.ReadKey().Key);
                Console.Clear();
            }
            switch (izbor)
            {
                case 81:                   
                    SviPutniNalozi.Clear();
                    SviZaposlenici.Clear();
                    SvaVozila.Clear();
                    SviIskoristeniZap.Clear();
                    Console.Clear();
                    Izbornik();
                    break;
                case 88:
                    Console.WriteLine("\n\n***IZLAZ***");
                    System.Threading.Thread.Sleep(2000);
                    System.Environment.Exit(1);
                    break;
            }

        }
        // Funkcija Citanje čita xml datoteku Putni_Nalozi.xml, čita sve putne naloge
        public static void Citanje()
        {
            string sXml = "";
            StreamReader oSr = new StreamReader("Putni_Nalozi.xml");
            using (oSr)
            {
                sXml = oSr.ReadToEnd();
            }
            XmlDocument oXml = new XmlDocument();
            oXml.LoadXml(sXml);
            XmlNodeList oNodes = oXml.SelectNodes("//data/SviPutniNalozi/putniNalog");
            foreach (XmlNode oNode in oNodes)
            {
                SviPutniNalozi.Add(new PutniNalog(Convert.ToInt32(oNode.Attributes["Rbr"].Value), oNode.Attributes["ime_i_prezime"].Value, oNode.Attributes["vrstaPrijevoza"].Value, oNode.Attributes["polaziste"].Value, oNode.Attributes["odrediste"].Value, oNode.Attributes["datum_o"].Value, oNode.Attributes["datum_p"].Value, oNode.Attributes["trajanje"].Value));
            }
        }
        // Funkcija CitanjeZaposlenika čita xml datoteku Putni_Nalozi.xml, čita sve zaposlenike
        public static void CitanjeZaposlenika()
        {
            var sXml = "";
            StreamReader oSr = new StreamReader("Putni_Nalozi.xml");
            using (oSr)
            {
                sXml = oSr.ReadToEnd();
            }
            XmlDocument oXml = new XmlDocument();
            oXml.LoadXml(sXml);
            XmlNodeList oNodes = oXml.SelectNodes("//data/SviZaposlenici/Zaposlenik");
            foreach (XmlNode oNode in oNodes)
            {
                SviZaposlenici.Add(new Zaposlenik(Convert.ToInt32(oNode.Attributes["id"].Value), oNode.Attributes["ime_i_prezime"].Value));
            }
        }
        // Funkcija CitanjeZaposlenika čita xml datoteku Putni_Nalozi.xml, čita sve zaposlenike
        public static void CitanjeIskoristenihZaposlenika()
        {
            var sXml = "";
            StreamReader oSr = new StreamReader("Putni_Nalozi.xml");
            using (oSr)
            {
                sXml = oSr.ReadToEnd();
            }
            XmlDocument oXml = new XmlDocument();
            oXml.LoadXml(sXml);
            XmlNodeList oNodes = oXml.SelectNodes("//data/SviZaposlenici/Zaposlenik");
            foreach (XmlNode oNode in oNodes)
            {
                SviIskoristeniZap.Add(new IskoristeniZaposlenici(Convert.ToInt32(oNode.Attributes["id"].Value), oNode.Attributes["ime_i_prezime"].Value, oNode.Attributes["OIB"].Value));
            }
        }
        // Funkcija CitanjeVozila čita xml datoteku Putni_Nalozi.xml, čita sva vozila
        public static List<TipVozila> CitanjeVozila()
        {
            string sXml = "";
            StreamReader oSr = new StreamReader("Putni_Nalozi.xml");
            using (oSr)
            {
                sXml = oSr.ReadToEnd();
            }
            XmlDocument oXml = new XmlDocument();
            oXml.LoadXml(sXml);
            XmlNodeList oNodes = oXml.SelectNodes("//data/TipPrijevoza/tip");            
            foreach (XmlNode oNode in oNodes)
            {
                SvaVozila.Add(new TipVozila(Convert.ToInt32(oNode.Attributes["ID"].Value), oNode.Attributes["vrsta"].Value));
            }
            return SvaVozila;
        }
        // Funkcija dajtipPoid vraca tip vozila tj. ID koji je jedinstven za svako vozilo 
        public static string dajtipPoid(int tip_id)
        {
            List<TipVozila> lTip = CitanjeVozila();
            string tip_tip = "";
            foreach (var tip in lTip)
            {
                if (tip.ID == tip_id)
                {
                    tip_tip = tip.Naziv;
                }
            }
            return tip_tip;
        }
        // Funkcija LogajTrenutnuAkciju zapisuje vrijeme u "Logovi.txt" kada je korisnik odabrao određenu akciju
        public static void LogajTrenutnuAkciju(string akcija)
        {
            DateTime now = DateTime.Now;
            StreamWriter log = new StreamWriter("Logovi.txt", true);
            log.WriteLine("{0} - {1}", now.ToString("F"), akcija);
            log.Flush();
            log.Close();
        }
        // Funkcija OcistiLogDatoteku briše sve iz "Logovi.txt"
        public static void OcistiLogDatoteku()
        {
            StreamWriter log = new StreamWriter("Logovi.txt", false);
            log.Flush();
            log.Close();
        }
        // Funkcija IspisSvihNaloga ispisuje sve putne naloge koji su upisani u XML datoteci Putni_Nalozi
        public static void IspisSvihNaloga()
        {
            LogajTrenutnuAkciju("Korisnik je odabrao ispis svih putnih naloga");
            Console.Clear();
            int rbr = 1;
            Citanje();
            var table = new ConsoleTable("R.br.", "IME I PREZIME", "VRSTA PRIJEEVOZA", "POLAZISTE", "ODREDISTE", "DATUM DOLASKA", "DATUM POVRATKA", "TRAJANJE");
            foreach (PutniNalog pojedinacniPutniNalog in SviPutniNalozi)
            {
                table.AddRow(rbr++ +".", pojedinacniPutniNalog.ImeIPrezime, pojedinacniPutniNalog.VPrijevoza, pojedinacniPutniNalog.Polaziste, pojedinacniPutniNalog.Odrediste, pojedinacniPutniNalog.DatumO, pojedinacniPutniNalog.DatumP, pojedinacniPutniNalog.Trajanje);            
            }
            table.Write();
        }
        // Funkcija IspisNalogaZaposlenika ispisuje naloge samo odabranog zaposlenika od strane korisnika
        public static void IspisNalogaZaposlenika()
        {
            LogajTrenutnuAkciju("Korisnik je odabrao ispis putnih naloga odredenog zaposlenika");
            Console.Clear();
            Citanje();
            CitanjeZaposlenika();
            CitanjeIskoristenihZaposlenika();
            foreach (var PojedinacniZaposlenik in SviZaposlenici) 
            {             
                Console.WriteLine("\n{0}.Ime i prezime: {1}\n", PojedinacniZaposlenik.id, PojedinacniZaposlenik.imePrezime);                                     
            }
            List<string> SviId = new List<string>();
            foreach (var Zaposlenik in SviIskoristeniZap)
            {
                SviId.Add(Zaposlenik.OsobniID);
            }          
            Console.WriteLine("\nUpisite OIB zaposlenika radi ispisa njegovih naloga:\n");
            string izborZaposlenika = Console.ReadLine();
            while (!SviId.Contains(izborZaposlenika))
            {
                Console.WriteLine("\nKrivi odabir");
                izborZaposlenika = Console.ReadLine();
            }
            Console.Clear();
            var table = new ConsoleTable("BROJ PUTNOG NALOGA", "VRSTA PRIJEEVOZA", "POLAZISTE", "ODREDISTE", "DATUM DOLASKA", "DATUM POVRATKA");
            string ImeIspis="";
            foreach (var Zaposlenik in SviIskoristeniZap)
            {                                                            
                if (izborZaposlenika == Zaposlenik.OsobniID)
                {
                    foreach (var PutniNalog in SviPutniNalozi)
                    {
                        if (Zaposlenik.ImeIPrezime == PutniNalog.ImeIPrezime)
                        {
                            ImeIspis = Zaposlenik.ImeIPrezime;
                            table.AddRow(PutniNalog.IDPutnogNaloga, PutniNalog.VPrijevoza, PutniNalog.Polaziste, PutniNalog.Odrediste, PutniNalog.DatumO, PutniNalog.DatumP);
                        }
                    }
                }                               
            }
            Console.WriteLine(ImeIspis);
            table.Write();            
        }
        // Funkcija StatistikaPrijevoznihSredstava računa broj korištenja zasebnog prijevoznog sredstva i ispisuje statistiku
        public static void StatistikaPrijevoznihSredstava()
        {
            LogajTrenutnuAkciju("Korisnik je odabrao ispis statistike prijevoznih sredstava");
            Console.Clear();
            Citanje();
            List<string> Vozila = new List<string>();
            List<string> IskoristenaVozila = new List<string>();
            var table = new ConsoleTable("R.br.", "PRIJEVOZNO SREDSTVO", "BROJ KORISTENJA");
            foreach (var TipPrijevoza in SviPutniNalozi)
            {
                if (!IskoristenaVozila.Contains(TipPrijevoza.VPrijevoza))
                {
                    Vozila.Add(TipPrijevoza.VPrijevoza);
                    IskoristenaVozila.Add(TipPrijevoza.VPrijevoza);
                }
            }
            int rbr = 1, brojkoristenja = 0;
            foreach (var VrstePrijevoza in Vozila)
            {
                foreach (var Vozilo in SviPutniNalozi)
                {
                    if (VrstePrijevoza == Vozilo.VPrijevoza)
                    {
                        brojkoristenja++;
                    }
                }
                table.AddRow(rbr++ +".", VrstePrijevoza, brojkoristenja);                               
                brojkoristenja = 0;
            }
            table.Write();
        }
        // Funkcija StatistikaPolazista računa broj zasebnih polazišta i ispisuje ih 
        public static void StatistikaPolazista()
        {
            LogajTrenutnuAkciju("Korisnik je odabrao ispis svih polazista i statistiku istih");
            Console.Clear();
            Citanje();
            List<string> Polazista = new List<string>();
            List<string> IskoritenaPolazista = new List<string>();
            var table = new ConsoleTable("R.br.", "POLAZISTE", "BROJ POLAZAKA");
            foreach (var Grad in SviPutniNalozi)
            {
                if (!IskoritenaPolazista.Contains(Grad.Polaziste))
                {
                    Polazista.Add(Grad.Polaziste);
                    IskoritenaPolazista.Add(Grad.Polaziste);
                }
            }
            int rbr = 1;
            int brojpolazaka = 0;
            foreach (var pojedinacnaPolazista in Polazista)// prolazi kroz sve gradove u listi polazista i ide u drugu foreach petlju
            {
                foreach (var Grad in SviPutniNalozi)// prolazi kroz sve gradove u putnim nalozima
                {
                    if (pojedinacnaPolazista == Grad.Polaziste)// ako je u listi polazista odabrani grad jednak gradu odabranog putnog naloga uci ce u if
                    {
                        brojpolazaka++;
                    }
                }
                table.AddRow(rbr++ + ".", pojedinacnaPolazista, brojpolazaka);                              
                brojpolazaka = 0;
            }
            table.Write();
        }
        // Funkcija StatistikaOdredista računa broj zasebnih odredišta i ispisuje ih
        public static void StatistikaOdredista()
        {
            LogajTrenutnuAkciju("Korisnik je odabrao ispis svih odredista i statistiku istih");
            Console.Clear();
            Citanje();
            List<string> Odredista = new List<string>();
            List<string> IskoritenaOdredista = new List<string>();
            var table = new ConsoleTable("R.br.", "ODREDISTE", "BROJ DOLAZAKA");
            foreach (var Odrediste in SviPutniNalozi)
            {
                if (!IskoritenaOdredista.Contains(Odrediste.Odrediste))
                {
                    Odredista.Add(Odrediste.Odrediste);
                    IskoritenaOdredista.Add(Odrediste.Odrediste);
                }
            }
            int rbr = 1;
            int brojdolazaka = 0;
            foreach (var pojedinacnaOdredista in Odredista)

            {
                foreach (var odrediste in SviPutniNalozi)
                {
                    if (pojedinacnaOdredista == odrediste.Odrediste)
                    {
                        brojdolazaka++;
                    }
                }
                table.AddRow(rbr++ + ".", pojedinacnaOdredista, brojdolazaka);               
                brojdolazaka = 0;
            }
            table.Write();
        }
        // Funkcija UpisZaposlenika traži od korisnika unos svih, prije određenih, parametara koje deklariraju zaposlenika i zapisuje ih u XML datoteku Putni_Nalozi.xml
        public static void UpisZaposlenika()
        {
            LogajTrenutnuAkciju("Korisnik je odabrao upis zaposlenika");
            Console.Clear();
            Pocetak:
            Console.WriteLine("\nDODAJ ZAPOSLENIKA\n");
            Console.WriteLine("Ime i Prezime:");
            string imePrezime = Console.ReadLine();
            Console.WriteLine("\nUpisi OIB(11 znamenaka): ");
            string oib = Console.ReadLine();
            CitanjeZaposlenika();
            CitanjeIskoristenihZaposlenika();
            int max = 1;
            List<int> IskoristeniID = new List<int>();
            List<string> IskoristeniZapOIB = new List<string>();
            foreach (var Zaposlenici in SviZaposlenici)
            {
                IskoristeniID.Add(Zaposlenici.id);
                if (IskoristeniID.Contains(max))
                {
                    max += 1;
                }
            }
            foreach (var UpisaniZap in SviIskoristeniZap)
            {
                IskoristeniZapOIB.Add(UpisaniZap.OsobniID);
                if (IskoristeniZapOIB.Contains(oib))
                {
                    Console.WriteLine("Zaposlenik vec postoji u bazi.");
                    Console.WriteLine("Q - Glavni izbornik\n");
                    Console.WriteLine("X - Izlaz\n");
                    Console.WriteLine("Y - Ponovi unos\n");
                    int izbor = Convert.ToInt32(Console.ReadKey().Key);
                    Console.Clear();
                    int[] dozvoljeniUnos = { 81, 88, 89 };
                    // Q -> 81
                    // X -> 88
                    // Y -> 89
                    while (!dozvoljeniUnos.Contains(izbor))
                    {
                        Console.WriteLine("\nNeispravan unos\n");
                        izbor = Convert.ToInt32(Console.ReadKey().Key);
                    }
                    switch (izbor)
                    {
                        case 81:
                            SviPutniNalozi.Clear();
                            SviZaposlenici.Clear();
                            SvaVozila.Clear();
                            SviIskoristeniZap.Clear();
                            Izbornik();
                            break;
                        case 88:
                            Console.WriteLine("\n***IZLAZ***");
                            System.Threading.Thread.Sleep(2000);
                            System.Environment.Exit(1);
                            break;
                        case 89:
                            IskoristeniID.Clear();
                            IskoristeniZapOIB.Clear();
                            goto Pocetak;
                    }
                }
            }
            string sXml = "";
            StreamReader oSr = new StreamReader(@"Putni_Nalozi.xml");
            using (oSr)
            {
                sXml = oSr.ReadToEnd();
            }
            XmlDocument oXml = new XmlDocument();
            oXml.LoadXml(sXml);
            XmlNode oNodes = oXml.SelectSingleNode("//data/SviZaposlenici");
            XmlElement xmlZaposlenici = oXml.CreateElement("Zaposlenik");
            xmlZaposlenici.SetAttribute("id", max.ToString());
            xmlZaposlenici.SetAttribute("ime_i_prezime", imePrezime);
            xmlZaposlenici.SetAttribute("OIB", oib);
            oNodes.AppendChild(xmlZaposlenici);
            oXml.Save(@"Putni_Nalozi.xml");
            Console.WriteLine("***ZAPOSLENIK USPJESNO DODAN***");
            System.Threading.Thread.Sleep(3000);
            Console.Clear();
        }       
        // Funkcija UpisNaloga traži od korisnika unos svih, preije određenih, paremaetara radi stvaranja novog punog naloga i zapusje ih u XML datoteku Putni_Nalozi.xml 
        public static void UpisNaloga()
        {
            LogajTrenutnuAkciju("Korisnik je odabrao izradu novog putnog naloga");
            Console.Clear();
            Pocetak:
            Console.WriteLine("\nNOVI PUTNI NALOG\n");
            Console.WriteLine("Ime i prezime zaposlenika:");
            string imePrezime = Console.ReadLine();
            List<string> UpisaniZaposlenici = new List<string>();
            CitanjeZaposlenika();
            Citanje();
            int max = 1;
            List<int> IskoristeniID = new List<int>();
            foreach (var Nalog in SviPutniNalozi)
            {
                IskoristeniID.Add(Nalog.IDPutnogNaloga);
                if (IskoristeniID.Contains(max))
                {
                    max += 1;
                }
            }
            foreach (var PojedinacniZaposlenik in SviZaposlenici)
            {
                UpisaniZaposlenici.Add(PojedinacniZaposlenik.imePrezime);
            }
            if (!UpisaniZaposlenici.Contains(imePrezime))
            {
                Console.WriteLine("\nZaposlenik kojeg ste upisali ne postoji u bazi.\n");
                Console.WriteLine("Molimo unesite zaposlenika putem foreme iz Glavnog izbornika.\nIli ponovite unos.\n");
                Console.WriteLine("Q - Glavni izbornik\n");
                Console.WriteLine("X - Izlaz\n");
                Console.WriteLine("Y - Dodaj zaposlenika i njegov novi putni nalog\n");
                int izbor = Convert.ToInt32(Console.ReadKey().Key);
                Console.Clear();
                int[] dozvoljeniUnos = { 81, 88, 89 };
                // Q -> 81
                // X -> 88
                // Y -> 89
                while (!dozvoljeniUnos.Contains(izbor))
                {
                    Console.WriteLine("\nNeispravan unos\n");
                    izbor = Convert.ToInt32(Console.ReadKey().Key);
                }
                switch (izbor)
                {
                    case 81:                        
                        SviPutniNalozi.Clear();
                        SviZaposlenici.Clear();
                        SvaVozila.Clear();
                        SviIskoristeniZap.Clear();
                        Izbornik();
                        break;
                    case 88:
                        Console.WriteLine("\n***IZLAZ***");
                        System.Threading.Thread.Sleep(2000);
                        System.Environment.Exit(1);
                        break;
                    case 89:
                        UpisZaposlenika();
                        UpisaniZaposlenici.Clear();
                        IskoristeniID.Clear();
                        goto Pocetak;                       
                }
            }
            Console.WriteLine("Tip: ");
            List<TipVozila> lTip = CitanjeVozila();

            int rbr = 1;
            foreach (var tip in lTip)
            {
                Console.WriteLine("\t" + rbr++ + ". " + tip.Naziv);
            }
            Console.Write("\nOdaberite tip: ");
            int tip_odabir = Convert.ToInt32(Console.ReadLine());

            int[] dozvoljeni_unosi = { 1, 2, 3, 4 };
            while (!dozvoljeni_unosi.Contains(tip_odabir))
            {
                Console.Write("\nPogreška pri odabiru tipa.Pokušajte ponovno ");
                Console.Write("\nOdaberite tip: ");
                tip_odabir = Convert.ToInt32(Console.ReadLine());
            }
            int odabrani_tip_id = lTip[tip_odabir - 1].ID;
            Console.WriteLine("\nUnesite polaziste:");
            string polaziste = Console.ReadLine();
            Console.WriteLine("\nUnesite odrediste");
            string odrediste = Console.ReadLine();

            CultureInfo MyCultureInfo = new CultureInfo("hr-HR");
            DateTime pocetak, kraj, SatiP, SatiK, MinP, MinK;
            Console.WriteLine("Unesite datum polaska u obliku DD/MM/YYYY: ");
            string StartDate = Console.ReadLine();
            while (StartDate.Length != 10)
            {
                Console.WriteLine("\nKrivi upis datuma, molimo ponovite!");
                StartDate = Console.ReadLine();
            }
            Console.WriteLine("Unesite sate polaska u obliku hh:00: ");
            string StartDateH = Console.ReadLine();
            while (StartDateH.Length != 5)
            {
                Console.WriteLine("\nKrivi upis sati, molimo ponovite!");
                StartDateH = Console.ReadLine();
            }
            Console.WriteLine("Unesite minute polaska u obliku 00:mm: ");
            string StartDateM = Console.ReadLine();
            while (StartDateM.Length != 5)
            {
                Console.WriteLine("\nKrivi upis minuta, molimo ponovite!");
                StartDateM = Console.ReadLine();
            }
            pocetak = DateTime.Parse(StartDate, MyCultureInfo);
            SatiP = DateTime.Parse(StartDateH, MyCultureInfo);
            MinP = DateTime.Parse(StartDateM, MyCultureInfo);

            Console.WriteLine("Unesite datum dolaska u obliku DD/MM/YYYY: ");
            string EndDate = Console.ReadLine();
            while (EndDate.Length != 10)
            {
                Console.WriteLine("\nKrivi upis datuma, molimo ponovite!");
                EndDate = Console.ReadLine();
            }
            Console.WriteLine("Unesite sate dolaska u obliku hh:00:");
            string EndDateH = Console.ReadLine();
            while (EndDateH.Length != 5)
            {
                Console.WriteLine("\nKrivi upis sati, molimo ponovite!");
                EndDateH = Console.ReadLine();
            }
            Console.WriteLine("Unesite minute dolaska u obliku 00:mm: ");
            string EndDateM = Console.ReadLine();
            while (EndDateM.Length != 5)
            {
                Console.WriteLine("\nKrivi upis minuta, molimo ponovite!");
                EndDateM = Console.ReadLine();
            }
            kraj = DateTime.Parse(EndDate, MyCultureInfo);
            SatiK = DateTime.Parse(EndDateH, MyCultureInfo);
            MinK = DateTime.Parse(EndDateM, MyCultureInfo);
            var UkDana = ((kraj - pocetak).TotalDays);
            var UkSati = ((SatiK - SatiP).TotalHours);
            var UkMin = ((MinK - MinP).TotalMinutes);
            if (SatiP > SatiK)
            {
                UkDana -= 1;
                UkSati += 24;
            }
            if (MinP > MinK)
            {
                UkSati -= 1;
                UkMin = (60 + UkMin);
            }            
            string DatumPolaska = StartDate + " " + StartDateH + " " + StartDateM;
            string ispis = (DatumPolaska.Remove(13, 9));
            var result = DatumPolaska.Substring(19);
            string SvePolazak = ispis + result;            
            string DatumDolaska = EndDate + " " + EndDateH + " " + EndDateM;
            string ispis1 = (DatumDolaska.Remove(13, 9));
            var result1 = DatumDolaska.Substring(19);
            string SveDolazak = ispis1 + result1;
            string Trajanje="";
            if(UkDana >= 1)
            {
               
                if(UkSati >= 12)
                {
                    UkDana += 1;
                }
                if (UkDana == 1 || UkDana == 21 || UkDana == 31)
                {
                    Trajanje = (UkDana.ToString() + " dan");
                }
                else
                {
                    Trajanje = (UkDana.ToString() + " dana");
                }
            }
            if (UkDana < 1)
            {
                if (UkSati == 1 || UkSati == 21)
                {
                    Trajanje = (UkSati.ToString() + " sat i " + UkMin.ToString() + " minuta.");
                }
                else if (UkSati == 2 || UkSati == 3 || UkSati == 4 || UkSati == 22 || UkSati == 23)
                {
                    Trajanje = (UkSati.ToString() + " sata i " + UkMin.ToString() + " minuta.");
                }
                else
                {
                    Trajanje = (UkSati.ToString() + " sati i " + UkMin.ToString() + " minuta.");
                }
            }
            string sXml = "";
            StreamReader oSr = new StreamReader(@"Putni_Nalozi.xml");
            using (oSr)
            {
                sXml = oSr.ReadToEnd();
            }
            XmlDocument oXml = new XmlDocument();
            oXml.LoadXml(sXml);
            XmlNode oNodes = oXml.SelectSingleNode("//data/SviPutniNalozi");
            XmlElement xmlPutni_Nalozi = oXml.CreateElement("putniNalog");
            xmlPutni_Nalozi.SetAttribute("Rbr", max.ToString());
            xmlPutni_Nalozi.SetAttribute("ime_i_prezime", imePrezime);            
            xmlPutni_Nalozi.SetAttribute("vrstaPrijevoza", dajtipPoid(odabrani_tip_id));
            xmlPutni_Nalozi.SetAttribute("polaziste", polaziste);
            xmlPutni_Nalozi.SetAttribute("odrediste", odrediste);
            xmlPutni_Nalozi.SetAttribute("datum_o", SvePolazak);
            xmlPutni_Nalozi.SetAttribute("datum_p", SveDolazak);            
            xmlPutni_Nalozi.SetAttribute("trajanje", Trajanje);
            oNodes.AppendChild(xmlPutni_Nalozi);
            oXml.Save(@"Putni_Nalozi.xml");
            Console.WriteLine("***PUTNI NALOG USPJESNO DODAN***");
            System.Threading.Thread.Sleep(3000);
            Console.Clear();
        }
        // Funkcija UserLogIn traži unos korisničkog imena i lozinke, ako se oba unosa podudaraju pročitanim iz XML datoteke config.xml korisnik se uspješno prijavio
        // i funkcija vraća true, ako jedan uvejt nije ispunjen funkcija vraća false
        public static bool UserLogIn()
        {
            Console.Clear();
            bool TocnoIme = false;
            bool TocanPass = false;

            List<LogIn> SviPodaciZaLogIn = new List<LogIn>();
            string sXml = "";
            StreamReader oSr = new StreamReader(@"config.xml");
            using (oSr)
            {
                sXml = oSr.ReadToEnd();
            }
            XmlDocument oXml = new XmlDocument();
            oXml.LoadXml(sXml);
            XmlNodeList oNodes = oXml.SelectNodes("//data/podatciLogIn");
            foreach (XmlNode oNode in oNodes)
            {
                SviPodaciZaLogIn.Add(new LogIn(oNode.Attributes["Ime"].Value, oNode.Attributes["Password"].Value));
            }
            Console.WriteLine("Unesite korisnocko ime:");
            string UnosIme = Console.ReadLine();
            Console.WriteLine("Unesite korisnocku lozinku:");
            string pass = "";
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Substring(0, (pass.Length - 1));
                        Console.Write("\b \b");
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                }
            } while (true);
            foreach (var podatakZaLogIn in SviPodaciZaLogIn)
            {               
                if (podatakZaLogIn.ime == UnosIme)
                {
                    TocnoIme = true;
                }
                if (podatakZaLogIn.pass == pass)
                {
                    TocanPass = true;
                }
            }
            if (TocnoIme == true && TocanPass == true)
            {
                Console.WriteLine("\nUspjesno ste se logirali");
                System.Threading.Thread.Sleep(750);
                Console.Clear();
                return true;
            }
            else
            {
                Console.WriteLine("\nNeuspjesna prijava");
                System.Threading.Thread.Sleep(750);
                Console.Clear();
                return false;
            }
        }
        // Funkcija ProgramStart pokreće program kod uspješne prijave
        public static void ProgramStart()
        {
            int prijavaBrojac = 0;
            bool UspjesnaPrijava = true;
            OcistiLogDatoteku();
            LogajTrenutnuAkciju("Korisnik je pokrenuo program.");
            while (prijavaBrojac < 3)
            {
                UspjesnaPrijava = UserLogIn();
                if (UspjesnaPrijava == false)
                {
                    prijavaBrojac++;
                    LogajTrenutnuAkciju("Korisnik nije unjeo dobru lozinku ili korisicko ime.");
                }
                else
                {
                    prijavaBrojac = 4;
                }
            }
            if (UspjesnaPrijava == true)
            {
                Izbornik();
            }
            else
            {
                Console.WriteLine("\nProgram se zatvara");                
                System.Threading.Thread.Sleep(2000);
                System.Environment.Exit(1);
            }
        }
        static void Main(string[] args)
        {
            ProgramStart();
            Console.ReadKey();            
        }
    }
}
