using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROGPOEY3.Data
{
    public class LinkedList<T>
    {
        public Node<T>? Head;
        public int Count = 0;

        public void Add(T value)
        {
            if (value == null)//Null check
            {
                return;
            } else if (Head == null)//Handle if list is empty
            {
                Head = new Node<T>(value);
                Count++;
                return;
            } else //Add new node to list
            {
                Node<T> n = new Node<T>(value);
                Node<T> current = Head;

                while (current.Next != null)
                {
                    current = current.Next;
                }

                n.Next = current;
                current.Next = n;
                Count++;
            }
        }

        public void Remove(T value)
        {
            if (Head == null)//Handle empty list
            {
                return;
            } else
            {
                Node<T> currentNode = Head;
                Node<T>? previousNode = null;

                while (currentNode != null)
                {
                    if (currentNode.Value.Equals(value))
                    {
                        // Remove the node
                        if (previousNode == null)
                        {
                            // Removing the head node
                            Head = currentNode.Next;
                        } else
                        {
                            previousNode.Next = currentNode.Next;
                        }

                        return; // Exit after removing the node
                    }

                    previousNode = currentNode;
                    currentNode = currentNode.Next;
                }
            }
        }
    }
}
