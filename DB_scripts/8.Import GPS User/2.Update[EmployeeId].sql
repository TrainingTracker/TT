UPDATE tt_capitalized.user
    SET EmployeeId = (case when UserName = 'amits' then '0502-00010'
                           when UserName = 'chinmoyp' then '0999-00001'
                           when UserName = 'sumitd' then '0705-00135'
                           when UserName = 'kirans' then '0705-00138'
                           when UserName = 'saswatb' then '0303-00018'
                           when UserName = 'amitabhp' then '0905-00158'
                    end)
    WHERE UserName in ('amits', 'chinmoyp', 'sumitd', 'kirans' , 'saswatb' , 'amitabhp')