//Rachel Hoffman
//Save string in a txt file.

#include <stdlib.h>
#include <iostream>
#include <fstream>
#include <string>

using namespace std;

const void storage(string json, string filename)
{
  ofstream file(filename.c_str());
  if (file.is_open())
  {
    file << json;
    file.close();
  }
  else cout << "Unable to open file";
}

const string readStorage(string filename)
{
  string line;
  //must .c_str to convert the string to a char
  ifstream myfile(filename.c_str());
  if (myfile.is_open())
  {
    while ( getline(myfile,line) )
    {
      cout << line << '\n';
    }
    
    myfile.close();
  }
  else cout << "Unable to open file";
  
  return line;
}

int main(int argc, char** argv)
{
    storage("{“a1”:”value”, “b2”:”89”, “c5”:”=b2+4”, “version”:”13”}\n", "example.txt");
    readStorage("example.txt");
}
