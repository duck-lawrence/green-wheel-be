export const MathDate = (dates) => {
    const { start, end } = dates
    if (!start || !end) return 0
    const a = new Date(start)
    const b = new Date(end)
    const diff = Math.ceil((+b - +a) / (1000 * 60 * 60 * 24))
    return Math.max(0, diff)
}
