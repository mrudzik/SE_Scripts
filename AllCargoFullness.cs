private IMyTextSurfaceProvider      _panelFront = null;
private List<IMyCargoContainer>    _myCargo = null;

private bool   InitComponents()
{
   _panelFront = GridTerminalSystem.GetBlockWithName("_TextPanelFront") as IMyTextSurfaceProvider;
   if (_panelFront == null)
      return false;

   _myCargo = new List<IMyCargoContainer>();
   GridTerminalSystem.GetBlocksOfType<IMyCargoContainer>(_myCargo);
   if (_myCargo == null)
       return false;
   Echo(_myCargo.Count.ToString());

   return true;
}



public void Main(string argument, UpdateType updateSource)

{
   if (!InitComponents())
       return;

   Echo("Yeee booi");
   string  statusText = "Cargo Containers : " + _myCargo.Count.ToString() + " in total\n\n";
   
   int i = 0;
   while (i < _myCargo.Count)
   {
       IMyInventory tempInv = _myCargo[i].GetInventory(0);        
       statusText += tempInv.MaxVolume.ToString() + " / " + tempInv.CurrentVolume.ToString() + "\n";
       i++;
   }


   _panelFront.GetSurface(0).WriteText(statusText);



}
