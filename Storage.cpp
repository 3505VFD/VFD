//Rachel Hoffman
//Save string in a txt file.

#include <stdlib.h>
#include <string>
#include <iostream>

const void Storage(std::string cellEdit)
{
    //“a1\t“79”\t14\n
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
}

int main(int argc, char** argv)
{
    Storage("a1\t79\t14\n");
}
