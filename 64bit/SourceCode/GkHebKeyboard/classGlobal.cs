using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GkHebKeyboard
{
    public class classGlobal
    {
        /*************************************************************************************************
         *                                                                                               *
         *                                           classGlobal                                         *
         *                                           ===========                                         *
         *                                                                                               *
         *  Purpose:                                                                                     *
         *  =======                                                                                      *
         *                                                                                               *
         *  Handles variabales that are global to the entire project.                                    *
         *                                                                                               *
         *  Variables:                                                                                   *
         *  =========                                                                                    *
         *                                                                                               *
         *  BasePath        This is the location holding all supporting files for normal processing.     *
         *                  It will be the location defined in the Registry settings.                    *
         *                                                                                               *
         *  AltBasePath     This is used specifically for development purposes and should not function   *
         *                  in live use.  It allows the application to run first time without any        *
         *                  Registry settings.                                                           *
         *                                                                                               *
         *  GreekProcessing This is simply a class which requires access from several classes.           *
         *                                                                                               *
         *************************************************************************************************/

        String basePath, altBasePath;
        classGreek greekProcessing;

        public string BasePath { get => basePath; set => basePath = value; }
        public string AltBasePath { get => altBasePath; set => altBasePath = value; }
        public classGreek GreekProcessing { get => greekProcessing; set => greekProcessing = value; }
    }
}
