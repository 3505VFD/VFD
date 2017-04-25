/*
 * Referenced chunks of code from:
 * http://www.linuxhowtos.org/C_C++/socket.htm
 * http://code.runnable.com/VXjZZimG7Nk0smWF/simple-tcp-server-code-for-c%2B%2B-and-socket
 */
#include <stdio.h>
#include <unistd.h>
#include <stdlib.h>
#include <string.h>
#include <sys/types.h> 
#include <sys/socket.h>
#include <netinet/in.h>


#include <iostream>
#include <fstream>
#include <boost/unordered_map.hpp>
#include <boost/foreach.hpp>

using namespace std;

/*
 * Function forward declarations
 */
void dostuff(int); 

const void saveFile(string,string);
const string readFile(string);
const void saveFilenames(boost::unordered_map<string,string>);
const boost::unordered_map<string,string> readFilenames();

void error(const char *msg)
{
    perror(msg);
    exit(1);
}

/*
 * MAIN
 */

 // Begin Skylar's server patchwork
int main(int argc, char *argv[])
{
     int sockfd, newsockfd, portno, pid;
     socklen_t clilen;
     struct sockaddr_in serv_addr, cli_addr;

     if (argc < 2) {
         fprintf(stderr,"ERROR, no port provided\n");
         exit(1);
     }

     sockfd = socket(AF_INET, SOCK_STREAM, 0);

     bzero((char *) &serv_addr, sizeof(serv_addr));
     portno = atoi(argv[1]);

     serv_addr.sin_family = AF_INET;
     serv_addr.sin_addr.s_addr = INADDR_ANY;
     serv_addr.sin_port = htons(portno);

     bind(sockfd, (struct sockaddr *) &serv_addr, sizeof(serv_addr));
              
     listen(sockfd,5);
     clilen = sizeof(cli_addr);

     while (true) {
         newsockfd = accept(sockfd, (struct sockaddr *) &cli_addr, &clilen);

         pid = fork();
         if (pid == 0)  {
             close(sockfd);
             dostuff(newsockfd);
             exit(0);
         }
         else 
         {
            close(newsockfd);
         }
     } 
     
     close(sockfd);
     return 0; 
}

/*
 * Split this into more functions (send, receive, etc.)
 * Rachel and Skylar both worked/tested this section
 */
void dostuff (int sock)
{
   int n;
   char buffer[256];
      
   bzero(buffer, 256);
   n = read(sock, buffer, 255);

   printf("Hello, %s\n", buffer);

   typedef boost::unordered_map<string,string> map;
   boost::unordered_map<string,string> fileNames = readFilenames();
   
   string files = "Send a filename from this list or a new filename: ";
   
   BOOST_FOREACH(map::value_type i, fileNames)
    {      
       files += i.first;
       files += "\t";
    }
    files = files.substr(0, files.size()-1);
    files += "\n";
    
    // send file names
    n = write(sock, files.c_str(), 255);
    
    // reset buffer to zero     
    bzero(buffer, 256);
    n = read(sock, buffer, 255);
    string filename = buffer;
    filename = filename.substr(0, filename.size()-1);
    
    // Test if recieve
    cout << filename << endl;
    
    // attempt to send json
    string json = readFile(filename);
    n = write(sock, json.c_str(), 255);
}


/***************************
 saving filenames and files
****************************/

// EVERYTHING BELOW IS RACHEL'S WORK
// Skylar tested the living daylights out of the buffers

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
const string readFile(string filename)
{
  // Append the .txt to the filename.
  filename +=".txt";
  string json;
  ifstream file(filename.c_str());
  if (file.is_open())
  {
      // Read the file for each line and puts that line into json.
      // NOTE: If only one json is being saved to file than this can be removed.
      while ( getline(file,json) )
      {
        // TEST: see if the file read the json
        //cout << json << '\n';
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
          key = line;
          line += ".txt";
          nameOfFiles[key] = line;
        }
      
        filesFind.close();
    }
    else cout << "Unable to open file";
    
    return nameOfFiles;
}

