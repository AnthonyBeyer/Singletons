# UnitySingletons
Monobehaviour and ScriptableObject Singletons for Unity

Base classes for Unity Singletons
---------------------------------

SoSingleton is best for : Save / Localization / Data Managers (Project)

MoSingleton is best for : GameObjects / Routines / Components Managers (In-Scene)

Usage :
-------

Declare your new class like this :

public class Localizer : SoSingleton<Localizer> 
{
  // Awesome stuff here
  public string GetMyAwesoneText()
  {
    return "WOW";
  }
}
  
Access to your singleton from anywhere like this :
Localizer.Instance.GetMyAwesoneText();
