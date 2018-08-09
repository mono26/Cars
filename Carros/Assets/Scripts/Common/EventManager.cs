using System;
using System.Collections.Generic;

public class CarEvent
{

}

public static class EventManager
{
    private static Dictionary<Type, List<EventHandlerBase>> events;

    static EventManager()
    {
        events = new Dictionary<Type, List<EventHandlerBase>>();
    }

    public static void AddListener<T>(EventHandler<T> listener) where T : CarEvent
    {
        if (!events.ContainsKey(typeof(T)))
            events[typeof(T)] = new List<EventHandlerBase>();

        if (!SubscriptionExists(typeof(T), listener))
            events[typeof(T)].Add(listener);

        return;
    }

    public static void RemoveListener<T>(EventHandler<T> listener) where T : CarEvent
    {
        Type eventType = typeof(T);

        if (!events.ContainsKey(eventType))
        {
#if EVENTROUTER_THROWEXCEPTIONS
			throw new ArgumentException( string.Format( "Removing listener \"{0}\", but the event type \"{1}\" isn't registered.", listener, eventType.ToString() ) );
#else
            return;
#endif
        }

        List<EventHandlerBase> subscriberList = events[typeof(T)];
        bool listenerFound;
        listenerFound = false;

        if (listenerFound)
        {

        }

        for (int i = 0; i < subscriberList.Count; i++)
        {
            if (subscriberList[i] == listener)
            {
                subscriberList.Remove(subscriberList[i]);
                listenerFound = true;

                if (subscriberList.Count == 0)
                    events.Remove(eventType);

                return;
            }
        }

#if EVENTROUTER_THROWEXCEPTIONS
		    if( !listenerFound )
		    {
			    throw new ArgumentException( string.Format( "Removing listener, but the supplied receiver isn't subscribed to event type \"{0}\".", eventType.ToString() ) );
		    }
#endif
    }

    private static bool SubscriptionExists<T>(Type type, EventHandler<T> receiver) where T : CarEvent
    {
        List<EventHandlerBase> receivers;

        if (!events.TryGetValue(type, out receivers)) return false;

        bool exists = false;

        for (int i = 0; i < receivers.Count; i++)
        {
            if (receivers[i] == receiver)
            {
                exists = true;
                break;
            }
        }

        return exists;
    }

    public static void TriggerEvent<T>(T newEvent) where T : CarEvent
    {
        List<EventHandlerBase> list;
        if (!events.TryGetValue(typeof(T), out list))
        {
#if EVENTROUTER_REQUIRELISTENER
	                    throw new ArgumentException( string.Format( "Attempting to send event of type \"{0}\", but no listener for this type has been found. Make sure this.Subscribe<{0}>(EventRouter) has been called, or that all listeners to this event haven't been unsubscribed.", typeof( MMEvent ).ToString() ) );
#else
            return;
#endif
        }

        for (int i = 0; i < list.Count; i++)
        {
            (list[i] as EventHandler<T>).OnEvent(newEvent);
        }
    }
}

public interface EventHandlerBase
{

}

public interface EventHandler<T> : EventHandlerBase
{
    void OnEvent(T eventType);
}
