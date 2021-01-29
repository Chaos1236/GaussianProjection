using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaussianProjection
{
    class Ellipsoid
    {
        public void Parameter(int ellipsoid, out double a, out double e2, out double epie2)
        {        
            a = new double();
            e2 = new double();
            epie2 = new double();
            switch (ellipsoid)
            {
                //CGCS2000
                case 1:
                    a = 6378137;
                    e2 = 0.0066943800229;
                    epie2 = 0.00673949677548;
                    break;
                //WGS-84椭球
                case 2:
                    a = 6378137;
                    e2 = 0.00669437999013;
                    epie2 = 0.00673949674227;
                    break;
                //1975国际椭球
                case 3:
                    a = 6378140;
                    e2 = 0.006694384999588;
                    epie2 = 0.006739501819473;
                    break;
                //克拉索夫斯基椭球
                case 4:
                    a = 6378245;
                    e2 = 0.006693421622966;
                    epie2 = 0.006738525414683;
                    break;             
            }
        }
    }
    class RadApplication
    {
        public int degree, minute, second;
        double degreeToRad = 180 * 3600 / Math.PI;
        public void AcceptDegree()
        {
            Console.WriteLine("请输入度°：");
            degree = int.Parse(Console.ReadLine());
            Console.WriteLine("请输入分′：");
            minute = int.Parse(Console.ReadLine());
            Console.WriteLine("请输入秒″：");
            second = int.Parse(Console.ReadLine());
        }
        public double GetDecimal()
        {
            return degree + minute / 60 + second / 3600;
        }
        public double GetRad()
        {
            double rad = (degree * 3600 + minute * 60 + second) / degreeToRad;
            return rad;
        }

    }
    class DegreeApplication
    {       
        public static void  Degree_app(double rad, out int degree, out int minute, out double second)
        {
            degree = Convert.ToInt32(Math.Truncate(rad * 180.0 / Math.PI));
            minute = Convert.ToInt32(Math.Truncate((rad * 180.0/ Math.PI * 3600.0 - degree * 3600.0) / 60));
            second = rad * 180.0 / Math.PI * 3600.0 - degree * 3600.0 - minute * 60.0;
            if (second==60)
            {
                minute += 1;
                second -= 60;
                if (minute==60)
                {
                    degree += 1;
                    minute -= 60;
                }
            }
        }
    }
    class BandZS
    {
        public double GetBand(double L)
        {
            Console.WriteLine("选择投影带：3°带请输入3，6°带请输入6");
            int band = int.Parse(Console.ReadLine());
            if (band==6)
            {
                double L0= 6 * (Math.Truncate(L / 6) + 1) - 3;
                return L0;
            }
            else
            {
                double L0= 3 * Math.Round(L / 3);
                return L0;
            }
        }
       
    }
    class BandFS
    {
        public double GetBand(int band, double y)
        {
            
            int n = (int)Math.Truncate(y / 1000000);
            if (band == 6)
            {
                return 6 * (n + 1) - 3;
            }
            else
            {
                return 3 * n;
            }
        }
    }

    
    class ZhenSuan
    {
        public void GetXY(out double x,out double y)
        {
            x = new double();
            y = new double();
            double a, e2, epie2;
            Console.WriteLine("请选择椭球：1 CGCS2000, 2 WGS-84椭球体， 3 1975年国际椭球体， 4 克拉索夫斯基椭球体");
            int ellipsoid = int.Parse(Console.ReadLine());
            Ellipsoid ellipsoid1 = new Ellipsoid();
            ellipsoid1.Parameter(ellipsoid, out a, out e2, out epie2);
            double m0 = a * (1.0 - e2);
            double m2 = 3.0 * e2 * m0 / 2.0;
            double m4 = 5.0 * e2 * m2 / 4.0;
            double m6 = 7.0 * e2 * m4 / 6.0;
            double m8 = 9.0 * e2 * m6 / 8.0;
            double a0 = m0 + m2 / 2.0 + 3.0 * m4 / 8.0 + 5.0 * m6 / 16.0 + 35.0 * m8 / 128.0;
            double a2 = m2 / 2.0 + m4 / 2.0 + 15.0 * m6 / 32.0 + 7.0 * m8 / 16.0;
            double a4 = m4 / 8.0 + 3.0 * m6 / 16.0 + 7.0 * m8 / 32.0;
            double a6 = m6 / 32.0 + m8 / 16.0;
            double a8 = m8 / 128.0;
            Console.WriteLine("m0:" + m0);
            Console.WriteLine("m2:" + m2);
            Console.WriteLine("m4:" + m4);
            Console.WriteLine("m6:" + m6);
            Console.WriteLine("m8:" + m8);
            Console.WriteLine("a0:" + a0);
            Console.WriteLine("a2:" + a2);
            Console.WriteLine("a4:" + a4);
            Console.WriteLine("a6:" + a6);
            Console.WriteLine("a8:" + a8);
            RadApplication Brad = new RadApplication();
            Console.WriteLine("请输入纬度");
            Brad.AcceptDegree();
            double Br = Brad.GetRad();
            Console.WriteLine(Br);
            double N = a / Math.Sqrt(1 - e2 * Math.Pow(Math.Sin(Br), 2));
            double t = Math.Tan(Br);
            double n2 = epie2 * Math.Pow(Math.Cos(Br), 2);
            RadApplication Lrad = new RadApplication();
            Console.WriteLine("请输入经度");
            Lrad.AcceptDegree();
            double Ldecimal = Lrad.GetDecimal();
            double Lr = Lrad.GetRad();
            Console.WriteLine(Lr);
            double X = a0 * Br - a2 * Math.Sin(2 * Br) / 2.0 + a4 * Math.Sin(4 * Br) / 4.0 - a6 * Math.Sin(6 * Br) / 6.0 + a8 * Math.Sin(8 * Br) / 8.0;
            Console.WriteLine("X:" + X);
            BandZS bandzs = new BandZS();
            double L0 = bandzs.GetBand(Ldecimal);
            Console.WriteLine("中央子午线经度为：" + L0);
            double l = Lr - L0*Math.PI/180;
            x = X + N / 2.0 * t * Math.Pow(Math.Cos(Br), 2) * l * l + N / 24.0 * t * (5.0 - t * t + 9.0 * n2 + 4.0 * n2 * n2) * Math.Pow(Math.Cos(Br), 4) * Math.Pow(l, 4) + N / 720.0 * t * (61 - 58 * t * t + Math.Pow(t, 4)) * Math.Pow(Math.Cos(Br), 6) * Math.Pow(l, 6);
            y = N * Math.Cos(Br) * l + N / 6.0 * (1 - t * t + n2) * Math.Pow(Math.Cos(Br), 3) * Math.Pow(l, 3) + N / 120.0 * (5 - 18 * t * t + Math.Pow(t, 4) + 14 * n2 - 58 * n2 * t * t) * Math.Pow(Math.Cos(Br), 5) * Math.Pow(l, 5);
            Console.WriteLine("x的值为：" + x);
            Console.WriteLine("y的值为：" + y);
        }

    }
    class FanSuan
    {
        public void GetBL(out double B,out double L)
        {           
            double a, e2, epie2;
            Console.WriteLine("请选择椭球：1 CGCS2000, 2 WGS-84椭球体， 3 1975年国际椭球体， 4 克拉索夫斯基椭球体");
            int ellipsoid = int.Parse(Console.ReadLine());
            Ellipsoid ellipsoid1 = new Ellipsoid();
            ellipsoid1.Parameter(ellipsoid, out a, out e2, out epie2);
            double m0 = a * (1 - e2);
            double m2 = 3 * e2 * m0 / 2;
            double m4 = 5 * e2 * m2 / 4;
            double m6 = 7 * e2 * m4 / 6;
            double m8 = 9 * e2 * m6 / 8;
            double a0 = m0 + m2 / 2 + 3 * m4 / 8 + 5 * m6 / 16 + 35 * m8 / 128;
            double a2 = m2 / 2 + m4 / 2 + 15 * m6 / 32 + 7 * m8 / 16;
            double a4 = m4 / 8 + 3 * m6 / 16 + 7 * m8 / 32;
            double a6 = m6 / 32 + m8 / 16;
            double a8 = m8 / 128;
            Console.WriteLine("请输入纵坐标X");
            double x=double.Parse(Console.ReadLine());
            Console.WriteLine("请输入横坐标Y");
            double Y=double.Parse(Console.ReadLine());
            double y = Y % 1000000 - 500000;
            BandFS bandfs=new BandFS();
            Console.WriteLine("选择投影带：3°带请输入3，6°带请输入6");
            int band = int.Parse(Console.ReadLine());
            double L0=bandfs.GetBand(band,Y);
            Console.WriteLine("L0" + L0);
            double Bf1 = x / a0;
            double F = -a2 / 2.0 * Math.Sin(2 * Bf1) + a4 / 4.0 * Math.Sin(4 * Bf1) - a6 / 6.0 * Math.Sin(6 * Bf1) + a8 / 8.0 * Math.Sin(8 * Bf1);
            double Bf2;
            while (true)
            {
                Bf2 = (x - F) / a0;
                if (Math.Abs(Bf2 - Bf1) <= Math.Pow(1, -10))
                {
                    break;
                }
                else
                {
                    Bf1 = Bf2;
                }
            }
            double Bf = Bf2;
            Console.WriteLine("Bf:" + Bf);
            double Vf = Math.Sqrt(1 + epie2 * Math.Pow(Math.Cos(Bf), 2));
            Console.WriteLine("Vf:" + Vf);
            double Wf = Math.Sqrt(1 - e2 * Math.Pow(Math.Sin(Bf), 2));
            Console.WriteLine("Wf:" + Wf);
            double Mf = a * (1 - e2) / Math.Pow(Wf, 3);
            double tf = Math.Tan(Bf);
            Console.WriteLine("tf:" + tf);
            double nf2 = Math.Sqrt(epie2 * Math.Pow(Math.Cos(Bf), 2));
            Console.WriteLine("nf2:" + nf2);
            double Nf = a / Wf;
            Console.WriteLine("Nf:" + Nf);
            double l = y / (Nf * Math.Cos(Bf)) - (1 + 2 * tf * tf) * Math.Pow(y, 3) / (6.0 * Math.Pow(Nf, 3) * Bf) + (5 + 28 * tf * tf + 24 * Math.Pow(tf, 4)) * Math.Pow(y, 5) / (120.0 * Math.Pow(Nf, 5) * Bf);
            B = Bf - 0.5 * Vf * Vf * tf * (Math.Pow(y / Nf, 2) - 1.0 / 12.0 * (5.0 + 3 * tf * tf + nf2 - 9.0 * nf2 * tf * tf) * Math.Pow(y / Nf, 4) + 1.0 / 360.0 * (61.0 + 90.0 * tf * tf + 45.0 * Math.Pow(tf, 2)) * Math.Pow(y / Nf, 6));
            //B = Bf - tf * y * y / (2.0 * Mf * Nf) + Math.Pow(tf, 3) * Math.Pow(y, 4) * (5.0 + 3.0 * tf * tf + nf2 - 9.0 * nf2 * tf * tf) / (24.0 * Mf * Math.Pow(Nf, 3));
            Console.WriteLine("B:" + B);
            Console.WriteLine("l:" + l);
            L = l + L0 * Math.PI / 180.0;
            Console.WriteLine("L:" + L);
            int Bdegree, Bminute;
            double Bsecond;
            DegreeApplication.Degree_app(B, out Bdegree, out Bminute, out Bsecond);
            Console.WriteLine("大地纬度B：{0}度{1}分{2}秒",Bdegree,Bminute,Bsecond);
            int Ldegree, Lminute;
            double Lsecond;
            DegreeApplication.Degree_app(L, out Ldegree, out Lminute, out Lsecond);
            Console.WriteLine("大地经度L：{0}度{1}分{2}秒",Ldegree,Lminute,Lsecond);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("高斯投影：1 高斯正算，2 高斯反算");
            int gaussian = int.Parse(Console.ReadLine());
            switch (gaussian)
            {
                case 1:
                    ZhenSuan ZS = new ZhenSuan();
                    double x, y;
                    ZS.GetXY(out x, out y);
                    break;
                case 2:
                    FanSuan FS = new FanSuan();
                    double B, L;
                    FS.GetBL(out B, out L);
                    break;
                default:
                    Console.WriteLine("ERROIR!");
                    break;
            }
            Console.ReadKey();
        }
    }
}
