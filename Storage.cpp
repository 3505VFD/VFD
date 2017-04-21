//Rachel Hoffman
/* This class gets a JSON string from the server and saves it to a text file.
 * The JSON string must be sent with a filname (no .txt) to save.
 * Reading the file will just need the name of the file and it returns the JSON string.
 */
 
/*NOTE:
 * "SpreadsheetFiles.txt" should never be overwritten.
 * Ask Skylar about file protection.
 * Boost libaray
 */

#include <stdlib.h>
#include <iostream>
#include <fstream>
#include <string>
#include <boost/unordered_map.hpp>
#include <boost/foreach.hpp>

using namespace std;

// SaveFile - saves a JSON string to a filename in .txt
const void saveFile(string json, string filename)
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

// readFile - reads a JSON string from a file and sends it.
// NOTE: Filename needs the .txt!!!
const string readFile(string filename)
{
  // Append the .txt to the filename.
  //filename +=".txt";
  string json;
  ifstream file(filename.c_str());
  if (file.is_open())
  {
      // Read the file for each line and puts that line into json.
      // NOTE: If only one json is being saved to file than this can be removed.
      while ( getline(file,json) )
      {
        // TEST: see if the file read the json
        cout << json << '\n';
      }
      
      file.close();
  }
  else cout << "Unable to open file";
  
  return json;
}


//UNORDERED MAP BOOST

// saveFilenames - saves a filenames in an unordered_map to file "SpreadsheetFiles"
const void saveFilenames(boost::unordered_map<string,string> nameOfFiles)
{
    typedef boost::unordered_map<string,string> map;
    ofstream filesFind("SpreadsheetFiles.txt");
    if (filesFind.is_open())
    {
        BOOST_FOREACH(map::value_type i, nameOfFiles)
        {
           filesFind << i.first<<"\n";
//           std::cout<<i.first<<"\n";
        }
        
        // Write json to file.
        filesFind.close();
    }
    else cout << "Unable to open file";
}


// readFilenames - Sends a unordered_map that contains file names saved to
// file "SpreadsheetFiles".
const boost::unordered_map<string,string> readFilenames()
{
    //unordered_map<Key= filename, Value= filename.txt>
    typedef boost::unordered_map<string,string> map;
    map nameOfFiles;
    string line, key;
    
    ifstream filesFind("SpreadsheetFiles.txt");
    if (filesFind.is_open())
    {
        // Read the file for each line and puts that line into json.
        // NOTE: If only one json is being saved to file than this can be removed.
        while ( getline(filesFind,line) )
        {
          // TEST: see if the file read the json
//          cout << line << '\n';
          key = line;
          line += ".txt";
          nameOfFiles[key] = line;
        }
      
        filesFind.close();
    }
    else cout << "Unable to open file";
    
    return nameOfFiles;
}


// TEST: See if both methods works or not.
int main(int argc, char** argv)
{
    //Spreadsheet
    //saveFile("{“a1”:”value”, “b2”:”89”, “c5”:”=b2+4”, “version”:”13”}\n", "example");
    //readFile("example.txt");
    
    //Name of Spreadsheet
    //create new unordered map
    typedef boost::unordered_map<string,string> map;
//    map nameOfFiles;
//    nameOfFiles["example"] = "example.txt";
//    nameOfFiles["taco"] = "taco.txt";
    
//    saveFilenames(nameOfFiles);
    boost::unordered_map<string,string> bread = readFilenames();
    
    BOOST_FOREACH(map::value_type i, bread)
    {
       std::cout<<i.first<<"\n";
    }
}
