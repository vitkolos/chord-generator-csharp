class ChordPlayer {
    // audio source: https://github.com/JamesStubbsEng/8ridgelite (GNU GPL)
    string path = Environment.CurrentDirectory + "\\audio";
    string folder = "Natural";
    // string folder = "Tuned";
    string[] sounds = { "0_e1.wav", "1_f1.wav", "2_fs1.wav", "3_g1.wav", "4_gs1.wav", "5_a1.wav", "7_as1.wav", "8_b1.wav", "9_c2.wav", "10_cs2.wav", "11_d2.wav", "12_ds2.wav", "13_e2.wav", "14_f2.wav", "15_fs2.wav", "16_g2.wav", "17_gs2.wav", "18_a2.wav", "19_as2.wav", "20_b2.wav", "21_c3.wav", "22_c3.wav", "23_d3.wav", "24_d3.wav", "25_e3.wav", "26_f3.wav", "27_fs3.wav", "28_g3.wav", "29_gs3.wav", "30_a3.wav", "31_as3.wav", "32_b3.wav", "33_c4.wav", "34_cs4.wav", "35_d4.wav", "36_ds4.wav", "37_e4.wav", "38_f4.wav", "39_fs4.wav", "40_g4.wav", "41_gs4.wav", "42_a4.wav", "43_as4.wav", "44_b4.wav", "45_c5.wav", "46_cs5.wav", "47_d5.wav", "48_ds5.wav", "49_e5.wav", "50_f5.wav", "51_fs5.wav", "52_g5.wav", "53_gs5.wav", "54_a5.wav", "55_as5.wav", "56_b5.wav", "57_c6.wav", "58_cs6.wav", "59_d6.wav", "60_ds6.wav", "61_e6.wav" };
    // string[] sounds = { "62_e1.wav", "63_f1.wav", "64_fs1.wav", "65_g1.wav", "66_gs1.wav", "67_a1.wav", "68_as1.wav", "69_b1.wav", "70_c2.wav", "71_cs2.wav", "72_d2.wav", "73_ds2.wav", "74_e2.wav", "75_f2.wav", "76_fs2.wav", "77_g2.wav", "78_gs2.wav", "79_a2.wav", "80_as2.wav", "81_b2.wav", "82_c3.wav", "83_cs3.wav", "84_d3.wav", "85_ds3.wav", "86_e3.wav", "87_f3.wav", "88_fs3.wav", "89_g3.wav", "90_gs3.wav", "91_a3.wav", "92_as3.wav", "93_b3.wav", "94_c4.wav", "95_cs4.wav", "96_d4.wav", "97_ds4.wav", "98_e4.wav", "99_f4.wav", "100_fs4.wav", "101_g4.wav", "102_gs4.wav", "103_a4.wav", "104_as4.wav", "105_b4.wav", "106_c5.wav", "107_cs5.wav", "108_d5.wav", "109_ds5.wav", "110_e5.wav", "111_f5.wav", "112_fs5.wav", "113_g5.wav", "114_gs5.wav", "115_a5.wav", "116_as5.wav", "117_b5.wav", "118_c6.wav", "119_cs6.wav", "120_d6.wav", "121_ds6.wav", "122_e6.wav" };
    int firstTone = 16;

    public void PlayChord(int[] stringPos, Instrument instrument) {
        for (int i = 0; i < stringPos.Length; i++) {
            var tone = stringPos[i] + instrument.realStrings[i] - firstTone;

            if (stringPos[i] != Position.mutedString && tone < sounds.Length && tone >= 0) {
                var p = new System.Windows.Media.MediaPlayer(); // this requires WPF
                p.Open(new System.Uri(path + "\\" + folder + "\\" + sounds[tone]));
                p.Play();
            }
        }
    }
}
