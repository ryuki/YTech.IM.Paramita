using System;
using System.Collections.Generic;
using System.Linq;
using YTech.IM.Paramita.Core.Master;

namespace YTech.IM.Paramita.Web.Controllers.Helper
{
    public static class Extensions<T>
    {
        //// Y Combinator generic implementation
        //private delegate Func<A, R> Recursive<A, R>(Recursive<A, R> r);
        //private static Func<A, R> Y<A, R>(Func<Func<A, R>, Func<A, R>> f)
        //{
        //    Recursive<A, R> rec = r => a => f(r(r))(a);
        //    return rec(rec);
        //}

        //// Extension method for IEnumerable<Item>
        //public static IEnumerable<MAccount> Traverse(this IEnumerable<MAccount> source, Func<MAccount, bool> predicate)
        //{
        //    var traverse = Extensions.Y<IEnumerable<MAccount>, IEnumerable<MAccount>>(
        //    f => items =>
        //    {
        //        var r = new List<MAccount>(items.Where(predicate));
        //        r.AddRange(items.SelectMany(i => f(i.Children)));
        //        return r;
        //    });

        //    return traverse(source);
        //}

        public static IEnumerable<T> Traverse(IEnumerable<T> source, Func<T, IEnumerable<T>> fnRecurse)
        {
            foreach (T item in source)
            {
                yield return item;

                IEnumerable<T> seqRecurse = fnRecurse(item);

                if (seqRecurse != null)
                {
                    foreach (T itemRecurse in Traverse(seqRecurse, fnRecurse))
                    {
                        yield return itemRecurse;
                    }
                }
            }
        }
    }
}
