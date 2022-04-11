﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cregennan.Core;
using Cregennan.Cryptography.Numerics;
using System.Diagnostics;
using System.Numerics;

namespace CregennanTests
{
    [TestClass]
    public class SoloveySchtrassen
    {
        [TestMethod]
        [Description("Тест простых чисел")]
        public void PrimeStabilityTest()
        {

            List<String> primes = new List<String>()
            {
                "74349117965329256087",
                "10243775205848656999",
                "12068134405291613239",
                "55757664349920470071",
                "26674514705450451779",
                "98437591053065779553",
                "32961643697755238353",
                "83892087119838552133",
                "28384389742770991703",
                "72121304097576015857",
                "26840307606805083707",
                "79412723712622286881",
                "58666971668489320429",
                "21815269229313770603",
                "15068067420418409777",
                "86764823942783259697",
                "31526487800692161287",
                "80344461486784062889",
                "28919414848583513047",
                "91924217983852206691"
            };
            SoloveySchtrassenVerifier solovey = new SoloveySchtrassenVerifier();

            foreach(var num in primes)
            {
                bool[] stability = new bool[5];
                for(int i = 0; i < 5; i++)
                {
                    stability[i] = solovey.Test(System.Numerics.BigInteger.Parse(num));
                }
                bool total = stability.Aggregate(true, (a, b) => a && b);
                Assert.IsTrue(total, "Число " + num + (total ? "" : " не") + " прошло тест");

            }
        }
    
        [TestMethod]
        [Description("Тест составных чисел")]
        public void CompositeStablilityTest()
        {
            List<String> primes = new List<String>()
            {
                "74349117965329256086",
                "10243775205848656995",
                "12068134405291613235",
                "55757664349920470072",
                "26674514705450451770",
                "98437591053065779555",
                "32961643697755238354",
                "83892087119838552134",
                "28384389742770991708",
                "72121304097576015852",
                "26840307606805083704",
                "79412723712622286882",
                "58666971668489320420",
                "21815269229313770605",
                "15068067420418409775",
                "86764823942783259696",
                "31526487800692161288",
                "80344461486784062880",
                "28919414848583513048",
                "91924217983852206692"
            };
            SoloveySchtrassenVerifier solovey = new SoloveySchtrassenVerifier();

            foreach (var num in primes)
            {
                bool[] stability = new bool[5];
                for (int i = 0; i < 5; i++)
                {
                    stability[i] = !solovey.Test(System.Numerics.BigInteger.Parse(num));
                }
                bool total = stability.Aggregate(true, (a, b) => a && b);
                Assert.IsTrue(total, "Число " + num + (total ? "" : " не") + " прошло тест");

            }
        }



        
        [TestMethod]
        [Description("Тест Символа Якоби")]
        public void JacobiSymbol()
        {
            var Tests = new List<(string, string, int)>()
            {
                ( "2663789754196785238221345975395408", "93105436369143989020119585641584811",  -1 ),
                ( "53449856434319440742732499649354866", "39122699144225389516421037244888821",  0 ),
                ( "92563201261163660033011782431422892", "3475911331533102069315206951180579",  -1 ),
                ( "12850590530868874242071193649294615", "40937419901759156656761464283738979",  1 ),
                ( "80287077901959400094460377076787566", "11420522031107733811154226227169395",  1 ),
                ( "79914707477394423670362808114709410", "31196049689921771548009868160092533",  -1 ),
                ( "87327489353360781463387091871965298", "49708108091858848997837769845997441",  0 ),
                ( "25453285341814075677787158530834114", "19285998193379909598879759846149285",  -1 ),
                ( "67579921783444799879044163994030992", "30495741747450552178034639270556583",  1 ),
                ( "36358018703053224371414118894222332", "15394957132615236064118757493348211",  1 ),
                ( "8686962702946223239751080099285054", "9726692854924295521662538680106381",  -1 ),
                ( "78683944712760554441114471521533802", "9899332662143540694019400597401479",  -1 ),
                ( "46388955358628331799059157293168854", "11228538741346392291765657115389979",  1 ),
                ( "51686615534341488949983893057965894", "7927000991375598980364772445027351",  1 ),
                ( "52289983672523661828805228376886453", "96485523550194263348659270877639403",  0 ),
                ( "55513668260560943766349507762710156", "46774603000548843296993157906919401",  0 ),
                ( "85239044833791905222997714693590287", "94332795689118718359996126827790931",  1 ),
                ( "51805747875379615679334823355592967", "19753866643068202477374657524657453",  1 ),
                ( "16994827532040701560156893516273791", "17313239220736126653807415681990627",  -1 ),
                ( "11534309500617734473278269143874294", "30951499455227472698428228886190501",  1 ),
            };

            

            foreach( (var a, var n, var t) in Tests)
            {
                Assert.AreEqual(Utils.JacobiSymbol(BigInteger.Parse(a), BigInteger.Parse(n)), t);
            }
        }
    }
}
