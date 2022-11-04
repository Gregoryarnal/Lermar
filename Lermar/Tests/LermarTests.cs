using System.Diagnostics;

using Montante;



namespace Tests;


[TestClass]
public class LermarTest
{
    [TestMethod]
    public void PalierTests()
    {
        APalierCmd palier = new APalierCmd(  nbPalierInt,  timePalierInt,  ifMaxPalierTxt,  gainResearchInt,  maxReachTxt,  chanceTxt,  attaqueTxt,  fromBallInt,  toBallInt,  fileNameTxt,  coinValueInt,  maxMiseInt, permanenceSelectedTxt, sauteuseValue, security, securityValue, typeOfMise);
        montanteManager = palier.getMontanteManager();
        palier.run();
    }
}