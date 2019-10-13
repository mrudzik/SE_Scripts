private string   _toShow;
private char[][] _pixArr; // Pixel Array
private int    size = 50;

public Program()

{

  Runtime.UpdateFrequency = UpdateFrequency.Update10;
   
   _pixArr = new char[size][];
   for (int y = 0; y < _pixArr.Length; y++)
   {
       _pixArr[y] = new char[size];
   }

}


private void ClearMonitor()
{
   for (int y = 0; y < _pixArr.Length; y++)
   {
       for (int x = 0; x < _pixArr[y].Length; x++)
       {
           _pixArr[y][x] = ' ';
       }
   }
}


private IMyTextSurface     _testPanel;
private List<IMyTextSurface>   _allMonitors;
private void   InitPanels()
{
   // Find TestPanel
   _testPanel = (GridTerminalSystem.GetBlockWithName("_TextPanelTest") as IMyTextSurfaceProvider).GetSurface(0);

   // Find all panels on grid
   _allMonitors = new List<IMyTextSurface>();
   GridTerminalSystem.GetBlocksOfType<IMyTextSurface>(_allMonitors);
   
   List<IMyCockpit> tempCockpits = new List<IMyCockpit>();
   GridTerminalSystem.GetBlocksOfType<IMyCockpit>(tempCockpits);
   for (int i = 0; i < tempCockpits.Count; i++)
   {
       IMyCockpit tempBlock = tempCockpits[i];
       for (int i2 = 0; i2 < tempBlock.SurfaceCount; i2++)
       {
           _allMonitors.Add(tempBlock.GetSurface(i2));
       }
   }

   List<IMyProgrammableBlock> tempProgs = new List<IMyProgrammableBlock>();
   GridTerminalSystem.GetBlocksOfType<IMyProgrammableBlock>(tempProgs);
   for (int i = 0; i < tempProgs.Count; i++)
   {
       IMyProgrammableBlock tempBlock = tempProgs[i];
       for (int i2 = 0; i2 < tempBlock.SurfaceCount; i2++)
       {
           _allMonitors.Add(tempBlock.GetSurface(i2));
       }
   }

   
}

private string TranslateToShow(char[][] myPixArr)
{
   string result = "";
   for (int y = 0; y < myPixArr.Length; y++)
   {
       for (int x = 0; x < myPixArr[y].Length; x++)
       {
           result += myPixArr[y][x];
       }
       result += "\n";
   }
   return result;
}

private void   FillAllMonitors()
{
   for (int i = 0; i < _allMonitors.Count; i++)
   {
       IMyTextSurface tempSurf = _allMonitors[i];
       
       tempSurf.ContentType = ContentType.TEXT_AND_IMAGE;
       tempSurf.WriteText(_toShow);
       tempSurf.FontSize = 1f;
       tempSurf.Font = "Monospace";
       tempSurf.FontColor = new Color (100, 190, 255);
   }
}











public void Main(string argument, UpdateType updateSource)

{
   InitPanels();
   ClearMonitor();

   RainMatrix(_pixArr);

   _toShow = TranslateToShow(_pixArr);
   FillAllMonitors();
   

}













// One color can only have 8 variations
static char rgb(int r, int g, int b) => (char)(0xe100 + (r << 6) + (g << 3) + b);
//toDisplay.Insert(toDisplay.Length - 1, rgb(i, i, i);


List<RainDrop> entireRain;
int    rainSize = 40;
int    rainFrequence = 2;
int    rainTimer = 0;
Random rnd;

class RainDrop
{
   public StringBuilder   text;
   public int        x;
   public int        y;

   public RainDrop(string newText, int newX, int newY)
   {
       this.text = new StringBuilder(newText);
       this.x = newX;
       this.y = newY;
   }
   
   public void     MoveDown()
   {
       y++;
       textTimer++;
       if (textTimer >= changeTime)
       {
           textTimer = 0;
           AlternateText();
       }
   }

   private int textTimer = 0;
   private int changeTime = 4;

   private void AlternateText()
   {
       for (int i = 0; i < text.Length; i++)
       {
           switch (text[i])
           {
case 'a':
   text[i] = '@';
   break;
case 'i':
   text[i] = '1';
   break;
case 'h':
   text[i] = 'X';
   break;
case 'c':
   text[i] = '(';
   break;
case 'e':
   text[i] = '3';
   break;
case ' ':
   text[i] = '_';
   break;
case 'o':
   text[i] = '0';
   break;
case 's':
   text[i] = '$';
   break;

case '@':
   text[i] = 'a';
   break;
case '1':
   text[i] = 'i';
   break;
case 'X':
   text[i] = 'h';
   break;
case '(':
   text[i] = 'c';
   break;
case '3':
   text[i] = 'e';
   break;
case '_':
   text[i] = ' ';
   break;
case '0':
   text[i] = 'o';
   break;
case '$':
   text[i] = 's';
   break;
           }
       }
   }
   
}

private void RainMatrix(char[][] myPixArr)
{
   if (entireRain == null)
       entireRain = new List<RainDrop>();
   if (rnd == null)
       rnd = new Random();

   
   rainTimer++;
   if (rainTimer >= rainFrequence)
   {
       rainTimer = 0;
       if (entireRain.Count < rainSize)
       {
           string dropText = ".";
           switch (rnd.Next(6))
           {
               case 0:
                   dropText = "u hacked";
                   break;
               case 1:
                   dropText = "hello there";
                   break;
               case 2:
                   dropText = "your grid bellongs to us";
                   break;
               case 3:
                   dropText = "surrender or die";
                   break;
               case 4:
                   dropText = "keep calm";
                   break;
               case 5:
                   dropText = "relax and enjoy";
                   break;
           }
           entireRain.Add(new RainDrop(dropText, rnd.Next(size), 0));
       }
   }

   
   for (int i = 0; i < entireRain.Count; i++)
   {
       RainDrop tempDrop = entireRain[i];
       tempDrop.MoveDown();
       
       // Print RainDrop on matrix
       for (int i2 = 0; i2 < tempDrop.text.Length; i2++)
       {
           int y = tempDrop.y + i2 - tempDrop.text.Length;
           int x = tempDrop.x;
           if (0 <= y && y < myPixArr.Length){
               if (0 <= x && x < myPixArr[y].Length){
                   myPixArr[y][x] = tempDrop.text[i2];
               }
           }   
       }

       // Remove if out of bounds
       if (tempDrop.y > size + tempDrop.text.Length)
       {
           entireRain.Remove(entireRain[i]);
           i--;
       }
   }
   
   
}




