namespace CiphersLibrary.Helpers
{
    /// <summary>
    /// Class for making stuff w/ matrix
    /// </summary>
    public static class MatrixHelper
    {
        /// <summary>
        /// Rotate Matrix right by 90 deg.
        /// </summary>
        /// <param name="MatrixToRotate"> Matrix</param>
        /// <param name="RowsAmnt"> Rows amount</param>
        /// <param name="ColsAmnt"> Columns amount</param>
        /// <returns>rotated matrix</returns>
        public static char[,] RotateRight(ref char[,] MatrixToRotate, int RowsAmnt, int ColsAmnt)
        {

            char[,] rightRotatedMatrix = new char[RowsAmnt, ColsAmnt];

            for (int i = 0; i < RowsAmnt; ++i)
            {
                for (int j = 0; j < ColsAmnt; ++j)
                {
                    rightRotatedMatrix[i,j] = MatrixToRotate[ColsAmnt - j - 1,i];
                }
            }

            return rightRotatedMatrix;
        }
    /// <summary>
    /// Rotate Matrix left by 90 deg.
    /// </summary>
    /// <param name="MatrixToRotate">Matrix</param>
    /// <param name="RowsAmnt">Rows amount</param>
    /// <param name="ColsAmnt">Columns amount</param>
    /// <returns>rotated matrix</returns>
    public static char[,] RotateLeft(ref char[,] MatrixToRotate, int RowsAmnt, int ColsAmnt)
    {

        char[,] leftRotatedMatrix = new char[RowsAmnt, ColsAmnt];

        for (int i = 0; i < ColsAmnt; ++i)
        {
            for (int j = 0; j < RowsAmnt; ++j)
            {
                leftRotatedMatrix[i, j] = MatrixToRotate[j, ColsAmnt - i - 1];
            }
        }

        return leftRotatedMatrix;
    }
}
}
