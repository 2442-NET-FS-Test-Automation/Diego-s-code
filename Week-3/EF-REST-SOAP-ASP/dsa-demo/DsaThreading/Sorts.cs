using System.IO.IsolatedStorage;

namespace DsaThreading;

public static class Sorts{


public static int[] Insertion(int[] input)
{
    
    int lenght = input.Length;

    //We need a for loop, and we'll start from the second element
    for(int i = 1; i < lenght; i++)
    {
        
        int key = input [i];
        int j = i - 1;

        //Shift elements of input that are grater than the key one position ahead
        //of where they are now
        while(j >= 0 && input[j] > key)
        {
            input[j + 1] = input[j];
            j--;
        }
        //insert the key into its sorted position
        input[j + 1] = key;
    }

    return input;
}


    //We find the smalles element from the usorted part of the array, and swap it
    //With the first unsorted element
public static int[] Selection(int[] input)
    {
        int lenght = input.Length;

        for(int i = 0; i < lenght - 1; i++)
        {
            
            //Assume the current position holds the min
            int min_index = i;

            //Iterate through the unsorted portion to find the actual minimum
            for(int j = i + 1; j < lenght; j++)
            {
                if(input[j] < input[min_index])
                {
                    //Update min_index if we find a smaller element
                    min_index = j;
                }
            }

            //Move the minimum element to its correct position
            int temp = input[i];
            input[i] = input[min_index];
            input[min_index] = temp;
        }

        return input;
    }


    //Merge sort - sort each half recursively then merge them
    public static int[] Merge(int[] input)
    {
        
        //Base case, if its an array of 1
        if (input.Length <= 1) return input;

        int mid = input.Length / 2;

        //We split the array into two halves
        int[] lef = Merge (input[..mid]);
        int[]right = Merge(input[mid..]);

        return MergeTwo(lef, right);
    }


    public static int[] MergeTwo(int[] Left, int[] right)
    {
        int[] sorted = new int[Left.Length + right.Length];

        int i = 0, j = 0, k = 0;


        //Traverse both array simultaneously, compare elements and 
        while(i < Left.Length && j < right.Length)
        {
            sorted[k++] = Left[i] <= right[j] ? Left[i++] : right[j++];
        }

        //Exhaust the remaining elementes from the Left array, if any are Left
        while(i < Left.Length) sorted[k++] = Left[i++];

        //Exhaust the remaining elements from the right array, if any are Left
        while(j < right.Length) sorted[k++] = right[j++];

        return sorted;
    }

}