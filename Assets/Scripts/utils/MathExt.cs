
public class MathExt {

    public static float PI = 3.1415927f;
    public static float TWO_PI = 6.2831855f;
    public static float HALF_PI = 1.5707964f;
    public static float QUARTER_PI = 0.7853982f;
  
    public static float Map(float value, float start1, float stop1, float start2, float stop2)
    {
        return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
    }

    // trigonometry
    public static float Radians(float degrees)
    {
        return degrees * PI / 180;
    }

    public static float Degrees(float radians)
    {
        return radians / PI * 180;
    }
    
  }
