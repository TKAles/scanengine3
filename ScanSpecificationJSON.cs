using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scanengine
{
    internal class ScanSpecificationJSON
    {
        public double scanEngine { get; set; }
        public int DetectionPower { get; set; }
        public int QCurrentFocus { get; set; }
        public int QCurrentScan {  get; set; }
        public decimal HeliosFrequency { get; set; }
        public decimal ScanOriginX { get; set; }
        public decimal ScanOriginY { get; set;}
        public decimal ScanDeltaX { get; set; }
        public decimal ScanDeltaY { get; set;}
        public int AnglesInScan {  get; set; }
        public decimal RowStepSize {  get; set; }
        public decimal StepsPerAngle { get; set; }
        public decimal ScanVelocity {  get; set; }
        public decimal ScanAcceleration { get; set; }
        public decimal DividerValue {  get; set; }
        public string T3COMPort {  get; set; }
        public string HeliosCOMPort {  get; set; }
        public string THORSerialNumber {  get; set; }
        public string OscilloscopeVISAAddress {  get; set; }
        public int StartingScanNumber {  get; set; }
        public bool IsRobometCampaign { get; set; }
        public int RobometLayers { get; set; }
        public int LayersInCampaign {  get; set; }

        public string ZaberPort { get; set; }
    }
}
