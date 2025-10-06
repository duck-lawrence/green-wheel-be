export const getDatesDiff = ({
    startDate,
    endDate
}: {
    startDate: string | null
    endDate: string | null
}) => {
    if (!startDate || !endDate) return 0
    const a = new Date(startDate)
    const b = new Date(endDate)
    const diff = Math.floor((+b - +a) / (1000 * 60 * 60 * 24))
    return Math.max(0, diff)
}
