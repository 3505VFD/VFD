/* A simple server in the internet domain using TCP
   The port number is passed as an argument 
   This version runs forever, forking off a separate 
   process for each connection
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

void dostuff(int); /* function prototype */
const void saveFile(string,string);
const string readFile(string);
const void saveFilenames(boost::unordered_map<string,string>);
const boost::unordered_map<string,string> readFilenames();

void error(const char *msg)
{
    perror(msg);
    exit(1);
}

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

     if (sockfd < 0) 
        error("ERROR opening socket");

     bzero((char *) &serv_addr, sizeof(serv_addr));
     portno = atoi(argv[1]);
     serv_addr.sin_family = AF_INET;
     serv_addr.sin_addr.s_addr = INADDR_ANY;
     serv_addr.sin_port = htons(portno);

     if (bind(sockfd, (struct sockaddr *) &serv_addr,
              sizeof(serv_addr)) < 0) 
              error("ERROR on binding");
              
     listen(sockfd,5);
     clilen = sizeof(cli_addr);

     while (1) {
         newsockfd = accept(sockfd, 
               (struct sockaddr *) &cli_addr, &clilen);

         if (newsockfd < 0) 
             error("ERROR on accept");

         pid = fork();

         if (pid < 0)
             error("ERROR on fork");

         if (pid == 0)  {
             close(sockfd);
             dostuff(newsockfd);
             exit(0);
         }
         else 
         {
            close(newsockfd);
         }
     } /* end of while */
     
     close(sockfd);
     return 0; /* we never get here */
}

/******** DOSTUFF() *********************
 There is a separate instance of this function 
 for each connection.  It handles all communication
 once a connnection has been established.
 *****************************************/
void dostuff (int sock)
{
   int n;
   char buffer[256];
      
   bzero(buffer, 256);
   n = read(sock, buffer, 255);

   if (n < 0) 
    error("ERROR reading from socket");
   printf("Here is the message: %s\n", buffer);
   
//   n = write(sock, "I got your message\n", 20);
//   if (n < 0) 
//    error("ERROR writing to socket");

   typedef boost::unordered_map<string,string> map;
   boost::unordered_map<string,string> fileNames = readFilenames();
   
   string files = "Send a filename from this list or a new filename\n";
   
   BOOST_FOREACH(map::value_type i, fileNames)
    {      
       files += i.first;
       files += "\t";
    }
    files = files.substr(0, files.size()-1);
    files += "\n";
    
    //Test for file names
    //cout << files << endl;
    
    n = write(sock, files.c_str(), files.length());
    if (n < 0) 
         error("ERROR writing to socket");
}


/**************
saving filenames and files
*******/


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

