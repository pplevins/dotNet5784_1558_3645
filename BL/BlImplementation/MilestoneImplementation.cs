using BlApi;
using BO;

namespace BlImplementation;

/// <summary>
/// Implementation of the <see cref="IMilestone"/> interface to manage Milestones in the BL.
/// </summary>
internal class MilestoneImplementation : IMilestone
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public void CreateProjectSchedule()
    {
        throw new NotImplementedException();
    }


    public Milestone? Read(int id)
    {
        throw new NotImplementedException();
    }

    public Milestone? Read(Func<Milestone, bool> filter)
    {
        throw new NotImplementedException();
    }

    public Milestone? Update(int id)
    {
        throw new NotImplementedException();
    }
}

