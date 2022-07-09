using System;

/**
 * Responsible to obtain current level system from the saved files.
 */
public class LevelHandler
{
    public LevelNode head;
    public LevelHandler()
    {
        head = ReadJson();
    }

    /**
     * Reads the pre-constructed json file
     * - Decodes the file, construct the level nodes accordingly and returns the head
     */
    private LevelNode ReadJson()
    {
        // TODO
        return new LevelNode(1);
    }
}
