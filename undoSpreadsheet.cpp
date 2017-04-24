//Rachel Hoffman

//This is for testing to create a section in server for undoing.

#include <stdlib.h>
#include <string>
#include <iostream>
#include <boost/unordered_map.hpp>
#include <boost/foreach.hpp>
#include <vector>

using namespace std;

int main(int argc, char** argv)
{

//****************Edit recived by server***********************//
    
    //“a1\t“79”\t14\n
    string cellEdit = "a1\t79\t14\n";
    
    //Separate the stuff.
    std::size_t found = cellEdit.find("\t");
    std::size_t next = cellEdit.find("\t", found+1,1);
    std::size_t end = cellEdit.find("\n");
     
    //Collect the data.
    std::string cell = cellEdit.substr(0,found);
    std::cout << cell << std::endl;
    
    std::string value = cellEdit.substr(found+1,next-found);
    std::cout << value << std::endl;
    
    std::string version = cellEdit.substr(next+1,end-next);
    std::cout << version << std::endl;
    
//****************check version***********************//
//****************test if it is being blocked***********************//
    
    
//****************See if value exists***********************//    
    
    vector<string> undo;
    string undoString;
    
    //must be created somewhere the server can acess it multiple times
    typedef boost::unordered_map<string,string> map;
    map Spreadsheet;
    
    if (Spreadsheet.find(cell) != Spreadsheet.end())
    {
        //If the key does exsits, copy it and the value and store it in the 'undo vector'.
        undoString=cell;
        undoString+="\t";
        undoString+=Spreadsheet[cell];
        undoString+="\t";
        undo.push_back(undoString); //NOTE: when sending back need to add the version and \n
        
        //Then change the value of that cell.
        Spreadsheet[cell] = value;
    }
    else
    {
        //Key does not exist, store the cell and a blank value ("") into the 'undo vector'.
        undoString=cell;
        undoString+="\t!!!!!\t"; //Not sure how to represent the empty section.
        undo.push_back(undoString); //NOTE: when sending back need to add the version and \n
        
        //Then insert new key and value.
        Spreadsheet[cell] = value;
    }
    
    value="=25+3";
    
    
    //*************TEST*****************//
    if (Spreadsheet.find(cell) != Spreadsheet.end())
    {
        //If the key does exsits, copy it and the value and store it in the 'undo vector'.
        undoString=cell;
        undoString+="\t";
        undoString+=Spreadsheet[cell];
        undoString+="\t";
        undo.push_back(undoString); //NOTE: when sending back need to add the version and \n
        
        //Then change the value of that cell.
        Spreadsheet[cell] = value;
    }
    else
    {
        //Key does not exist, store the cell and a blank value ("") into the 'undo vector'.
        undoString=cell;
        undoString+="\t!!!!!\t"; //Not sure how to represent the empty section.
        undo.push_back(undoString); //NOTE: when sending back need to add the version and \n
        
        //Then insert new key and value.
        Spreadsheet[cell] = value;
    }
    
    //*************TEST*****************//
    
    for (vector<string>::iterator it = undo.begin() ; it != undo.end(); ++it)
        std::cout << *it << '\n';
    
}
