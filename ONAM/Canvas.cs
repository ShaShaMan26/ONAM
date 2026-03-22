namespace ONAM;

public class Canvas
{
    private class Node
    {
        public Node next;
        public GameElement value;

        public Node(GameElement e)
        {
            value = e;
            next = null;
        }
        public Node(GameElement e, Node next)
        {
            value = e;
            this.next = next;
        }
    }

    private Node[] layers;

    public virtual void Initialize()
    {
        layers = new Node[10];
    }

    public void Add(int layer, GameElement gameElement)
    {
        if (layers[layer] == null)
        {
            layers[layer] = new Node(gameElement);
        }
        else
        {
            layers[layer] = new Node(gameElement, layers[layer]);
        }
    }
    public void Add(GameElement gameElement)
    {
        Add(4, gameElement);
    }

    public virtual void Draw()
    {
        foreach (Node head in layers)
        {
            for (Node n = head; n != null; n = n.next)
            {
                n.value.Draw();
            }
        }
    }
}
