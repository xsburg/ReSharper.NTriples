using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Secret.Util
{
    internal static class PsiTreeUtil
    {
        /// <summary>Looks for a parent of specified type and returns null if the node is not found.</summary>
        /// <param name="element">The element to operate on.</param>
        /// <param name="maxDistance">The max distance to find. Use 0 to look only this item, 1 to look the direct parent only etc.</param>
        public static ITreeNode GetParent<T>(this ITreeNode element, int maxDistance)
        {
            int distance = 0;
            while (element != null && distance <= maxDistance)
            {
                if (element is T)
                {
                    return element;
                }

                element = element.Parent;
                distance++;
            }

            return null;
        }
    }
}