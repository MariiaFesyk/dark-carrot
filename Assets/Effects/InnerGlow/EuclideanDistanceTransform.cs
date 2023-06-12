using UnityEngine;

// https://prideout.net/blog/distance_fields/
public static class EuclideanDistanceTransform {
    public struct Sampler2D {
        public int columns, rows;
        public float[] data;

        public float Sample(int x, int y){
            return data[Mathf.Clamp(x, 0, columns - 1) + Mathf.Clamp(y, 0, rows - 1) * columns];
        }
        public float Sample(float x, float y){
            int ix = Mathf.FloorToInt(x);
            int iy = Mathf.FloorToInt(y);
            float fx = x - ix;
            float fy = y - iy;

            return Mathf.Lerp(
                Mathf.Lerp(Sample(ix, iy), Sample(ix + 1, iy), fx),
                Mathf.Lerp(Sample(ix, iy + 1), Sample(ix + 1, iy + 1), fx), fy
            );
        }
    }

    struct ArraySlice<T> {
        public T[] array;
        public int offset, stride, length;

        public int Length => length;
        public T this[int index] {
            get => array[offset + index * stride];
            set => array[offset + index * stride] = value;
        }
    }

    public static Sampler2D compute(bool[] mask, int columns, int rows, bool invert){
        var field = new int[mask.Length];
        for(int c = 0; c < columns; c++) for(int r = 0; r < rows; r++)
            field[c + r * columns] = mask[c + r * columns] == invert ? int.MaxValue : 0;

        ArraySlice<int> slice = new ArraySlice<int> { array = field };

        slice.stride = 1;
        slice.length = columns;
        for(int r = 0; r < rows; r++){
            slice.offset = r * columns;
            march_parabolas(slice);
        }
        slice.stride = columns;
        slice.length = rows;
        for(int c = 0; c < columns; c++){
            slice.offset = c;
            march_parabolas(slice);
        }

        var data = new float[field.Length];
        for(int i = 0; i < field.Length; i++) data[i] = Mathf.Sqrt(field[i]);
        return new Sampler2D { data = data, columns = columns, rows = rows };
    }
    private static void march_parabolas(ArraySlice<int> row){
        int[] hull_vertices_x = new int[row.Length];
        int[] hull_vertices_y = new int[row.Length];
        float[] hull_intersections = new float[row.Length + 1];

        int k = 0;
        hull_vertices_x[0] = 0;
        hull_vertices_y[0] = row[0];
        hull_intersections[0] = -1;
        hull_intersections[1] = float.MaxValue;
        for(int i = 1; i < row.Length; i++){
            int j = hull_vertices_x[k];
            var s = ((float) row[i] + i*i - (float) row[j] - j*j) / (2f * i - 2f * j);
            while(k > 0 && s <= hull_intersections[k]){
                k = k - 1;
                j = hull_vertices_x[k];
                s = ((float) row[i] + i*i - (float) row[j] - j*j) / (2f * i - 2f * j);
            }
            k = k + 1;
            hull_vertices_x[k] = i;
            hull_vertices_y[k] = row[i];
            hull_intersections[k] = s;
            hull_intersections[k + 1] = float.MaxValue;
        }
        k = 0;
        for(int i = 0; i < row.Length; i++){
            while(hull_intersections[k + 1] < i) k = k + 1;
            int dx = i - hull_vertices_x[k];
            row[i] = dx * dx + hull_vertices_y[k];
        }
    }
}
