namespace LibrarySolution.Application.Interfaces;


public interface IUnitOfWork
{
    /// <summary>
    /// 변경사항을 데이터베이스에 저장합니다.
    /// </summary>
    /// <param name="cancellationToken">취소 토큰</param>
    /// <returns>변경점이 적용된 Row의 수</returns>
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
