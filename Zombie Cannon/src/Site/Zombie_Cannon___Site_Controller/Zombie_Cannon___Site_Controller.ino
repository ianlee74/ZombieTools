/* Zombie Cannon Site Controller */

const int LAYER_COUNT = 4;
const int layer[] = { 4 /* red */
                     ,5 /* yellow */
                     ,7 /* green */
                     ,6 /* green */ };

const int ROW_COUNT = 8;
const int row[] = {10, 9, 11, 8, 12, 3, 13, 2};

void setup() {

  for(int l = 0; l < LAYER_COUNT; l++)
  {
    pinMode(layer[l], OUTPUT);
  }
  
  for(int r = 0; r < ROW_COUNT; r++)
  {
    pinMode(row[r], OUTPUT);
  }
}

void loop()
{
  switch(GetCurrentCommand())
  {
    case 0:
      Randomizer(10);
      break;
    case 1:
      Crosshair(1);      
      break;
    case 2:
      Crosshair(2);   
      break;   
    case 3: 
      Crosshair(3); 
      break;     
    default:
      Radar(10, 3, 1, 3);
  }
}

byte GetCurrentCommand()
{
  int lowBit = analogRead(4);    
  int highBit = analogRead(5); 

  byte returnVal = 0;
  if(lowBit > 10) returnVal += 1;
  if(highBit > 10) returnVal += 2;  

  return returnVal;
}

void Crosshair(byte length)
{
  LightLine(1, 0, length);  
  LightLine(1, 2, length);  
  LightLine(1, 4, length);  
  LightLine(1, 6, length);  
}

void Randomizer(int delayTime)
{
  byte layerNum = random(LAYER_COUNT); 
  byte rowNum = random(ROW_COUNT);
  BlinkSingleLight(delayTime, rowNum, layerNum);
}

void BlinkSingleLight(int delayTime, int rowNum, int layerNum)
{
  digitalWrite(row[rowNum], HIGH);
  digitalWrite(layer[layerNum], HIGH);
  delay(delayTime);
  digitalWrite(row[rowNum], LOW);
  digitalWrite(layer[layerNum], LOW);
}

void Radar(int delayTime, int lineRedraw, byte clockwise, byte length)
{
  for(int r=0; r < ROW_COUNT; r++)
  {
    for(int rep=0; rep < lineRedraw; rep++)
    {
      LightLine(delayTime, r, length);
    }
    digitalWrite(row[r], LOW);
  }  
}

void LightLine(int delayTime, int line, byte length)
{
  if(length < 1 || length > LAYER_COUNT) length = LAYER_COUNT;
  
  const int CENTER = 1;
  // Always make sure the center light is lit.
  if(line != CENTER)
  {
    digitalWrite(row[CENTER], HIGH);
    digitalWrite(layer[0], HIGH);
    delay(delayTime);
    digitalWrite(layer[0], LOW);    
    digitalWrite(row[CENTER], LOW);
  }
  // Light the intended line.
  digitalWrite(row[line], HIGH);
  for(int l = 0; l < length; l++)
  {
    digitalWrite(layer[l], HIGH);
    delay(delayTime);
    digitalWrite(layer[l], LOW);
  }
  digitalWrite(row[line], LOW);
}

void Chase(int delayTime, int layerNum)
{
  digitalWrite(layer[layerNum], HIGH);
  for(int r = 0; r < ROW_COUNT; r++)
  {
    digitalWrite(row[r], HIGH);
    delay(delayTime);
    digitalWrite(row[r], LOW);
  }  
  digitalWrite(layer[layerNum], LOW);
}

void LightAll(int delayTime)
{
  for(int l = 0; l < LAYER_COUNT; l++)
  {
    digitalWrite(layer[l], HIGH);
    for(int r = 0; r < ROW_COUNT; r++)
    {
      digitalWrite(row[r], HIGH);  
      delay(delayTime);
      digitalWrite(row[r], LOW);
    }
    digitalWrite(layer[l], LOW);
  }   
}
