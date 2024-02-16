# Usage de l'implémentation acctuelle :

Il suffit de créer la classe et de s'abonné a l'évenement:
```csharp
PostureRightHandUp postureRightHandUp = new PostureRightHandUp();
postureRightHandUp.GestureRecognized += OnPostureRightHandUpRecognized;
private void OnPostureRightHandUpRecognized(object sender, GestureRecognizedEventArgs e)
{
    // Logique déclanché par l'évenement
}
```
On aurais pu créer le ``GestureManager`` mais nous n'avons pas eu le temps