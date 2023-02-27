using System.Collections;

namespace scrutin;

public class Scrutin
{
    public bool isSecondRound = false;

    private bool isClosed = false;
    private Dictionary<int, int> votes = new Dictionary<int, int>();

    private int winner;

    private bool isTie = false;
    public void setCandidat(int candidat, int nbVotes)
    {
        if (isClosed) throw new Exception("Sondage cloturé");
        votes.Add(candidat, nbVotes);
    }

    public void close()
    {
        isClosed = true;
        if (!isSecondRound)
        {
            winner = votes.Keys.ToList().Find(x => getPourcentage(x) > 50);
            if (winner != 0) return;
            
            var w1 = 0;
            var w2 = 0;

            foreach (var c in votes.Keys)
            {
                var v = getVotes(c);
                if (w1 == 0 || v > getVotes(w1)) w1 = c;
                else if (w2 == 0 || v > getVotes(w2)) w2 = c;
            }

            votes = new Dictionary<int, int>();
            votes.Add(w1, 0);
            votes.Add(w2, 0);
            isSecondRound = true;
        }
        else
        {
            winner = votes.Keys.ToList().Find(x => getPourcentage(x) > 50);
            if (winner == 0) isTie = true;
        }
        
    }

    public int getPourcentage(int candidat)
    {
        try
        {
            return (int)Math.Round((double)(100 * votes[candidat]) / votes.Values.Sum());
        }
        catch (Exception e)
        {
            return 0;
        }
    }

    public int getVotes(int candidat)
    {
        return votes[candidat];
    }

    public bool isInVote(int candidat)
    {
        return votes.ContainsKey(candidat);
    }

    public void setSecondRound()
    {
        isSecondRound = true;
    }
    
    public int getWinner()
    {
        
        if (!isClosed) throw new Exception("Sondage non cloturé");
        if (isTie) throw new Exception("Tie");
        return winner;

    }
}