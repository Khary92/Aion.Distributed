raceCollector updateCollector,
    IAddTicketToCurrentSprintTraceCollector addTicketToSprintCollector) : ITicketUseCaseSelector
{
    public ICreateTicketTraceCollector Create => createCollector;
    public IUpdateTicketTraceCollector Update => updateCollector;
    public IAddTicketToCurrentSprintTraceCollector AddTicketToSprint => addTicketToSprintCollector;
}