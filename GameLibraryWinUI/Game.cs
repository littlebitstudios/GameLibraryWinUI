using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Pickers.Provider;

namespace GameLibraryWinUI
{
    internal class Game
    {
        public string name = "";
        public string developer = "";
        public string description = "";
        public string imagepath = "";
        public string genre = "";
        public string filename = "";

        public Game(string name, string developer, string description, string imagepath, string genre, string filename)
        {
            this.name = name;
            this.developer = developer;
            this.description = description;
            this.imagepath = imagepath;
            this.genre = genre;
            this.filename = filename;
        }

        public Game(string name, string developer, string description, string genre, string filename)
        {
            this.name = name;
            this.developer = developer;
            this.description = description;
            this.genre = genre;
            this.filename = filename;
        }

        public Game()
        {

        }
    }
}