﻿/*
Copyright (c) 2014, Lars Brubaker
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met: 

1. Redistributions of source code must retain the above copyright notice, this
   list of conditions and the following disclaimer. 
2. Redistributions in binary form must reproduce the above copyright notice,
   this list of conditions and the following disclaimer in the documentation
   and/or other materials provided with the distribution. 

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

The views and conclusions contained in the software and documentation are those
of the authors and should not be interpreted as representing official policies, 
either expressed or implied, of the FreeBSD Project.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using MatterHackers.Agg;
using MatterHackers.Agg.UI;
using MatterHackers.VectorMath;
using MatterHackers.Agg.Font;
using MatterHackers.Localizations;
using MatterHackers.MatterControl.SlicerConfiguration;

namespace MatterHackers.MatterControl
{
    public class LevelWizard2Point : LevelWizardBase
    {
        Vector2 position0 = new Vector2(0, 0);
        Vector2 position1 = new Vector2(220, 0);

        string pageOneStepText = "Print Leveling Overview".Localize();
        string pageOneInstructionsTextOne = LocalizedString.Get("Welcome to the print leveling wizard. Here is a quick overview on what we are going to do.");
        string pageOneInstructionsTextTwo = LocalizedString.Get("'Home' the printer");
        string pageOneInstructionsTextThree = LocalizedString.Get("Sample the bed at two points");
        string pageOneInstructionsTextFour = LocalizedString.Get("Turn auto leveling on");
        string pageOneInstructionsText5 = LocalizedString.Get("You should be done in about 2 minutes.");
        string pageOneInstructionsText6 = LocalizedString.Get("Note: Be sure the tip of the extrude is clean.");
        string pageOneInstructionsText7 = LocalizedString.Get("Click 'Next' to continue.");

        public LevelWizard2Point(RuningState runningState)
            : base(500, 370, 6)
        {
            string printLevelWizardTitle = LocalizedString.Get("MatterControl");
            string printLevelWizardTitleFull = LocalizedString.Get("Print Leveling Wizard");
            Title = string.Format("{0} - {1}", printLevelWizardTitle, printLevelWizardTitleFull);
            ProbePosition[] probePositions = new ProbePosition[2];
            probePositions[0] = new ProbePosition();
            probePositions[1] = new ProbePosition();

            printLevelWizard = new WizardControl();
            AddChild(printLevelWizard);

            if (runningState == RuningState.InitialStartupCalibration)
            {
                string requiredPageInstructions = "{0}\n\n{1}".FormatWith(requiredPageInstructions1, requiredPageInstructions2);
                printLevelWizard.AddPage(new FirstPageInstructions(initialPrinterSetupStepText, requiredPageInstructions));
            }

            string pageOneInstructions = string.Format("{0}\n\n\t• {1}\n\t• {2}\n\t• {3}\n\n{4}\n\n{5}\n\n{6}", pageOneInstructionsTextOne, pageOneInstructionsTextTwo, pageOneInstructionsTextThree, pageOneInstructionsTextFour, pageOneInstructionsText5, pageOneInstructionsText6, pageOneInstructionsText7);
            printLevelWizard.AddPage(new FirstPageInstructions(pageOneStepText, pageOneInstructions));

            string homingPageInstructions = string.Format("{0}:\n\n\t• {1}\n\n{2}", homingPageInstructionsTextOne, homingPageInstructionsTextTwo, homingPageInstructionsTextThree);
            printLevelWizard.AddPage(new HomePrinterPage(homingPageStepText, homingPageInstructions));

            Vector2 probeFrontLeft = position0;
            string positionLabelTwo = LocalizedString.Get("Position");
            string lowPrecisionTwoLabel = LocalizedString.Get("Low Precision");
            string medPrecisionTwoLabel = LocalizedString.Get("Medium Precision");
            string highPrecisionTwoLabel = LocalizedString.Get("High Precision");
            printLevelWizard.AddPage(new GetCoarseBedHeightProbeFirst(printLevelWizard, new Vector3(probeFrontLeft, 10), string.Format("{0} {1} 2 - {2}", GetStepString(), positionLabelTwo, lowPrecisionTwoLabel), probePositions[0]));
            printLevelWizard.AddPage(new GetFineBedHeight(string.Format("{0} {1} 2 - {2}", GetStepString(), positionLabelTwo, medPrecisionTwoLabel), probePositions[0]));
            printLevelWizard.AddPage(new GetUltraFineBedHeight(string.Format("{0} {1} 2 - {2}", GetStepString(), positionLabelTwo, highPrecisionTwoLabel), probePositions[0]));

            Vector2 probeFrontRight = position1;
            string positionLabelThree = LocalizedString.Get("Position");
            string lowPrecisionLabelThree = LocalizedString.Get("Low Precision");
            string medPrecisionLabelThree = LocalizedString.Get("Medium Precision");
            string highPrecisionLabelThree = LocalizedString.Get("High Precision");
            printLevelWizard.AddPage(new GetCoarseBedHeightProbeFirst(printLevelWizard, new Vector3(probeFrontRight, 10), string.Format("{0} {1} 3 - {2}", GetStepString(), positionLabelThree, lowPrecisionLabelThree), probePositions[1]));
            printLevelWizard.AddPage(new GetFineBedHeight(string.Format("{0} {1} 3 - {2}", GetStepString(), positionLabelThree, medPrecisionLabelThree), probePositions[1]));
            printLevelWizard.AddPage(new GetUltraFineBedHeight(string.Format("{0} {1} 3 - {2}", GetStepString(), positionLabelThree, highPrecisionLabelThree), probePositions[1]));

            string doneInstructions = string.Format("{0}\n\n\t• {1}\n\n{2}", doneInstructionsText, doneInstructionsTextTwo, doneInstructionsTextThree);
            printLevelWizard.AddPage(new LastPage2PointInstructions("Done", doneInstructions, probePositions));
        }
    }
}
