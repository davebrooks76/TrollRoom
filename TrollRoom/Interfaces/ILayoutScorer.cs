namespace TrollRoom
{
    public interface ILayoutScorer
    {
        double Score(Map map, Layout layout);
    }
}