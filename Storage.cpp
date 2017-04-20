//Rachel Hoffman
/* This class gets a JSON string from the server and saves it to a text file.
 * The JSON string must be sent with a filname (no .txt) to save.
 * Reading the file will just need the name of the file and it returns the JSON string.
 */ 

#include <stdlib.h>
#include <iostream>
#include <fstream>
#include <string>

using namespace std;

// Storage - saves a JSON string to a filename in .txt
const void storage(string json, string filename)
{
  // Append the .txt to the filename.
  filename +=".txt";
  ofstream file(filename.c_str());
  if (file.is_open())
  {
    // Write json to file.
    file << json;
    file.close();
  }
  else cout << "Unable to open file";
}

// readStorage - reads a JSON string from a file and sends it.
const string readStorage(string filename)
{
  // Append the .txt to the filename.
  filename +=".txt";
  string json;
  ifstream myfile(filename.c_str());
  if (myfile.is_open())
  {
    // Read the file for each line and puts that line into json.
    // NOTE: If only one json is being saved to file than this can be removed.
    while ( getline(myfile,json) )
    {
      // TEST: see if the file read the json
      cout << json << '\n';
    }
    
    myfile.close();
  }
  else cout << "Unable to open file";
  
  return json;
}

// TEST: See if both methods works or not.
int main(int argc, char** argv)
{
    storage("{“a1”:”value”, “b2”:”89”, “c5”:”=b2+4”, “version”:”13”}\n", "example");
    readStorage("example");
}
